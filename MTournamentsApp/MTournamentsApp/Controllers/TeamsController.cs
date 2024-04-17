using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MTournamentsApp.Entities;
using MTournamentsApp.Models;
using System.IO;
using System.Numerics;

namespace MTournamentsApp.Controllers
{
    public class TeamsController : Controller
    {
        private TournamentsDbContext _tournamentsDbContext;

        public TeamsController(TournamentsDbContext tournamentsDbContext)
        {
            _tournamentsDbContext = tournamentsDbContext;
        }

        public IActionResult List()
        {
            return View(GetTeams());
        }

        private List<Team> GetTeams()
        {
            List<Team> teams = _tournamentsDbContext.Teams.Include(t => t.MainTeamGame).ToList();

            foreach (var team in teams)
            {
                team.Players = _tournamentsDbContext.Players.Where(p => p.TeamId == team.TeamId).OrderBy(p => p.Id).ToList();
                team.Tournaments = _tournamentsDbContext.Tournaments.Where(t => t.TeamIds!.Contains(team.TeamId!)).ToList();
            }

            return teams;
        }

        [Authorize()]
        [HttpGet()]
        public IActionResult Add()
        {
            List<Game> games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();

            return View(new TeamViewModel() { Team = new Team(), GamesList = games, PlayersList = null });
        }

        [HttpPost("Teams/REST/Add")]
        public async Task<IActionResult> RESTAdd([FromBody] Team team)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _tournamentsDbContext.Teams.AddAsync(team);
                await _tournamentsDbContext.SaveChangesAsync();

                return Ok(new { teamId = team.TeamId });
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

        [HttpGet("Teams/REST/List")]
        public async Task<IActionResult> RESTList()
        {
            var teamsList = _tournamentsDbContext.Teams
                .Select(t => new
                {
                    t.TeamId,
                    t.TeamName,
                    t.TeamDescription,
                    t.MainTeamGameId,
                    t.PlayerIds,
                    t.TournamentIds
                })
                .ToList();

            return Ok(new { teams = teamsList, total = teamsList.Count() });
        }

        [Authorize()]
        [HttpPost()]
        public IActionResult Add(TeamViewModel t)
        {
            if (ModelState.IsValid)
            {
                Game game = _tournamentsDbContext.Games.Where(g => g.GameId == t.Team.MainTeamGameId).FirstOrDefault();

                Game customGame;

                if (game == null)
                {
                    string gameId;
                    if (t.Team.MainTeamGame.GameName.Contains(" "))
                    {
                        var gameIdWords = t.Team.MainTeamGame.GameName.Split(' ');
                        var gameIdParts = gameIdWords.Select(word => word.Substring(0, Math.Min(word.Length, 3)));
                        gameId = string.Join("", gameIdParts);
                    }
                    else
                    {
                        gameId = t.Team.MainTeamGame.GameName.Substring(0, Math.Min(t.Team.MainTeamGame.GameName.Length, 3));
                    }

                    customGame = new Game() { GameId = gameId, GameName = t.Team.MainTeamGame.GameName };
                    _tournamentsDbContext.Games.Add(customGame);

                    t.Team.MainTeamGame = customGame;
                    t.Team.MainTeamGameId = gameId;
                }
                else
                {
                    t.Team.MainTeamGame = game;
                    t.Team.MainTeamGameId = game.GameId;
                }

                var teamIdWords = t.Team.TeamName.Split(' ');
                var teamIdParts = teamIdWords.Select(word => word.Substring(0, Math.Min(word.Length, 3)));
                t.Team.TeamId = string.Join("", teamIdParts);

                _tournamentsDbContext.Teams.Add(t.Team);
                _tournamentsDbContext.SaveChanges();

                return RedirectToAction("List", "Teams");
            }
            else
            {
                List<Game> games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();

                return View(new TeamViewModel() { Team = new Team(), GamesList = games, PlayersList = null });
            }
        }

        [Authorize()]
        [HttpGet()]
        public IActionResult Edit(string id)
        {
            var games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
            var players = _tournamentsDbContext.Players.OrderBy(p => p.FirstName).ToList();
            var team = _tournamentsDbContext.Teams
                        .Include(t => t.Players)
                        .Include(t => t.Tournaments)
                        .FirstOrDefault(t => t.TeamId == id);

            if (team == null)
            {
                return RedirectToAction("List", "Teams");
            }

            var viewModel = new TeamViewModel()
            {
                GamesList = games,
                PlayersList = players,
                Team = team
            };

            return View(viewModel);
        }

        [Authorize()]
        [HttpPost()]
        public IActionResult Edit(TeamViewModel t)
        {
            if (ModelState.IsValid)
            {
                _tournamentsDbContext.Teams.Update(t.Team);
                _tournamentsDbContext.SaveChanges();

                return RedirectToAction("List", "Teams");
            }
            else
            {
                t.PlayersList = _tournamentsDbContext.Players.ToList();
                t.GamesList = _tournamentsDbContext.Games.ToList();
                return View(t);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public IActionResult Delete(string id)
        {
            var team = _tournamentsDbContext.Teams.Include(t => t.MainTeamGame).Where(t => t.TeamId == id).FirstOrDefault();
            team.Players = _tournamentsDbContext.Players.Where(p => p.TeamId == team.TeamId).OrderBy(p => p.Id).ToList();
            team.Tournaments = _tournamentsDbContext.Tournaments.Where(t => t.TeamIds!.Contains(team.TeamId!)).ToList();

            if (team != null)
            {
                return View(team);
            }
            else
            {
                return RedirectToAction("List", "Teams");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public IActionResult Delete(Team t)
        {
            var players = _tournamentsDbContext.Players.Where(p => p.TeamId == t.TeamId).ToList();

            foreach (var player in players)
            {
                player.TeamId = null;
                _tournamentsDbContext.SaveChanges();
            }

            _tournamentsDbContext.Teams.Remove(t);
            _tournamentsDbContext.SaveChanges();

            return RedirectToAction("List", "Teams");
        }

        [HttpGet()]
        public IActionResult Members(string id)
        {
            List<Player> players = _tournamentsDbContext.Players.Include(p => p.Role).Include(p => p.Team).Where(p => p.TeamId == id).OrderBy(p => p.Id).ToList();

            if (players != null)
            {
                return View(players);
            }
            else
            {
                return RedirectToAction("List", "Teams");
            }
        }
    }
}
