using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MTournamentsApp.Entities;
using MTournamentsApp.Models;
using MTournamentsApp.Services;
using System.IO;
using System.Linq;

namespace MTournamentsApp.Controllers
{
    public class TournamentsController : Controller
    {
        private IMail _mail;
        private TournamentsDbContext _tournamentsDbContext;

        public TournamentsController(IMail mail, TournamentsDbContext tournamentsDbContext)
        {
            _mail = mail;
            _tournamentsDbContext = tournamentsDbContext;
        }

        public IActionResult List()
        {
            return View(getTournaments());
        }

        [Authorize()]
        [HttpGet()]
        public IActionResult Add()
        {
            List<Game> games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
            List<Team> teams = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();

            return View(new TournamentViewModel() { Tournament = new Tournament(), GamesList = games, TeamsList = teams });
        }

        [Authorize()]
        [HttpPost]
        public IActionResult Add(TournamentViewModel t, List<string> SelectedTeamIds)
        {
            t.GamesList = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
            t.TeamsList = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();

            if (!ModelState.IsValid || SelectedTeamIds.Count < 2)
            {
                if (SelectedTeamIds.Count < 2)
                {
                    ModelState.AddModelError("SelectedTeamIds", "Please select at least two teams for the tournament.");
                }
                return View(t);
            }

            Tournament newTournament = new Tournament
            {
                TournamentName = t.Tournament.TournamentName,
                TournamentDate = t.Tournament.TournamentDate,
                Address = t.Tournament.Address,
                TournamentGameId = t.Tournament.TournamentGameId,
                TeamIds = SelectedTeamIds
            };

            _tournamentsDbContext.Tournaments.Add(newTournament);

            _tournamentsDbContext.SaveChanges();

            return RedirectToAction("List");
        }


        private List<Tournament> getTournaments()
        {
            List<Tournament> tournaments = _tournamentsDbContext.Tournaments.Include(t => t.TournamentGame).ToList();

            foreach (var tournament in tournaments)
            {
                tournament.TournamentTeams = _tournamentsDbContext.Teams.Where(t => t.TournamentIds.Contains(tournament.Id)).OrderBy(t => t.TeamId).ToList();
            }

            return tournaments;
        }

        [HttpGet()]
        public IActionResult Add()
        {
            List<Game> games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
            List<Team> teams = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();
            Tournament newTournament = new Tournament() { Address = new Address() };

            return View(new TournamentViewModel() { Tournament = newTournament, GamesList = games, TeamsList = teams });
        }

        [HttpGet("Tournaments/REST/List")]
        public async Task<IActionResult> RESTList()
        {
            var tournamentsList = _tournamentsDbContext.Tournaments
                .Include(t => t.Address)
                .Select(t => new
                {
                    t.Id,
                    t.TournamentName,
                    t.TournamentDate,
                    t.TournamentGameId,
                    t.Address,
                    t.TeamIds,
                })
                .ToList();

            return Ok(new { tournaments = tournamentsList, total = tournamentsList.Count() });
        }

        [HttpPost("Tournaments/REST/Add")]
        public async Task<IActionResult> RESTAdd([FromBody] TournamentRequest tournamentRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _tournamentsDbContext.Addresses.AddAsync(tournamentRequest.Address);
                await _tournamentsDbContext.SaveChangesAsync();

                tournamentRequest.Tournament.AddressID = tournamentRequest.Address.Id;

                await _tournamentsDbContext.Tournaments.AddAsync(tournamentRequest.Tournament);
                await _tournamentsDbContext.SaveChangesAsync();

                var teamIds = tournamentRequest.Tournament.TeamIds;

                List<Team> teams = _tournamentsDbContext.Teams.Where(t => teamIds.Contains(t.TeamId)).ToList();

                foreach (var team in teams)
                {
                    if (team.TournamentIds.IsNullOrEmpty())
                    {
                        team.TournamentIds = new List<int>();
                    }
                    if (team.Tournaments.IsNullOrEmpty())
                    {
                        team.Tournaments = new List<Tournament>();
                    }
                    team.TournamentIds.Add(tournamentRequest.Tournament.Id);
                    team.Tournaments.Add(tournamentRequest.Tournament);
                    _tournamentsDbContext.Teams.Update(team);
                }

                _tournamentsDbContext.SaveChanges();

                return Ok(new { tournamentId = tournamentRequest.Tournament.Id });
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

        [HttpPost()]
        public IActionResult Add(TournamentViewModel tvm, List<string> SelectedTeamIds)
        {
            if (!ModelState.IsValid || SelectedTeamIds.Count < 2)
            {
                if (SelectedTeamIds.Count < 2)
                {
                    ModelState.AddModelError("SelectedTeamIds", "Please select at least two teams for the tournament.");
                }

                List<Game> g = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
                List<Team> t = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();

                tvm.GamesList = g;
                tvm.TeamsList = t;

                return View(tvm);
            }

            Address? address = _tournamentsDbContext.Addresses.Where(a => a.StreetAddress == tvm.Tournament.Address.StreetAddress && a.TournamentCity == tvm.Tournament.Address.TournamentCity && a.TournamentCountry == tvm.Tournament.Address.TournamentCountry).FirstOrDefault();
            Address customAddress;
            if (address == null)
            {
                customAddress = new Address() { StreetAddress = tvm.Tournament.Address.StreetAddress, TournamentCity = tvm.Tournament.Address.TournamentCity, TournamentCountry = tvm.Tournament.Address.TournamentCountry, TournamentPostalCode = tvm.Tournament.Address.TournamentPostalCode };
                _tournamentsDbContext.Addresses.Add(customAddress);

                tvm.Tournament.Address = customAddress;
                tvm.Tournament.AddressID = customAddress.Id;
            }
            else
            {
                tvm.Tournament.AddressID = address.Id;
                tvm.Tournament.Address = address;
            }

            _tournamentsDbContext.Tournaments.Add(tvm.Tournament);
            _tournamentsDbContext.SaveChanges();


            tvm.Tournament.TeamIds = SelectedTeamIds;

            List<Team> teams = _tournamentsDbContext.Teams.Where(t => SelectedTeamIds.Contains(t.TeamId)).ToList();

            foreach (var team in teams)
            {
                if (team.TournamentIds.IsNullOrEmpty())
                {
                    team.TournamentIds = new List<int>();
                }
                if (team.Tournaments.IsNullOrEmpty())
                {
                    team.Tournaments = new List<Tournament>();
                }
                team.TournamentIds.Add(tvm.Tournament.Id);
                team.Tournaments.Add(tvm.Tournament);
                _tournamentsDbContext.Teams.Update(team);
            }

            _tournamentsDbContext.SaveChanges();

            return RedirectToAction("List", "Tournaments");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tournament = _tournamentsDbContext.Tournaments
                .Include(t => t.Address)
                .Include(t => t.TournamentTeams)
                .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound($"No tournament found with ID {id}.");
            }

            var games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
            var teams = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();

            var viewModel = new TournamentViewModel
            {
                Tournament = tournament,
                GamesList = games,
                TeamsList = teams,
                SelectedTeamIds = tournament.TeamIds
            };

            return View(viewModel);
        }


        [HttpPost()]
        public IActionResult Edit(TournamentViewModel tvm)
        {
            if (tvm.SelectedTeamIds == null)
            {
                tvm.SelectedTeamIds = new List<string>();
            }

            var existingTournament = _tournamentsDbContext.Tournaments
                .Include(t => t.TournamentTeams)
                .Include(t => t.Address)
                .FirstOrDefault(t => t.Id == tvm.Tournament.Id);

            if (existingTournament == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || tvm.SelectedTeamIds.Count < 2)
            {
                ModelState.AddModelError("SelectedTeamIds", "Please select at least two teams for the tournament.");

                tvm.GamesList = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
                tvm.TeamsList = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();
                tvm.Tournament = existingTournament;

                return View(tvm);
            }

            existingTournament.TournamentName = tvm.Tournament.TournamentName;
            existingTournament.TournamentGameId = tvm.Tournament.TournamentGameId;
            existingTournament.TournamentDate = tvm.Tournament.TournamentDate;
            existingTournament.Address.StreetAddress = tvm.Tournament.Address.StreetAddress;
            existingTournament.Address.TournamentCity = tvm.Tournament.Address.TournamentCity;
            existingTournament.Address.TournamentCountry = tvm.Tournament.Address.TournamentCountry;
            existingTournament.Address.TournamentPostalCode = tvm.Tournament.Address.TournamentPostalCode;

            var currentTeamIds = existingTournament.TeamIds.ToList();
            var teamsToAdd = tvm.SelectedTeamIds.Except(currentTeamIds).ToList();
            var teamsToRemove = currentTeamIds.Except(tvm.SelectedTeamIds).ToList();

            foreach (var teamId in teamsToAdd)
            {
                var team = _tournamentsDbContext.Teams.Include(t => t.Tournaments).FirstOrDefault(t => t.TeamId == teamId);
                if (team != null)
                {
                    team.Tournaments.Add(existingTournament);
                    existingTournament.TeamIds.Add(teamId);
                    existingTournament.TournamentTeams.Add(team);
                    _tournamentsDbContext.Teams.Update(team);
                }
            }

            foreach (var teamId in teamsToRemove)
            {
                var team = _tournamentsDbContext.Teams.Include(t => t.Tournaments).FirstOrDefault(t => t.TeamId == teamId);
                if (team != null)
                {
                    team.Tournaments.Remove(existingTournament);
                    existingTournament.TeamIds.Remove(teamId);
                    existingTournament.TournamentTeams.Remove(team);
                    _tournamentsDbContext.Teams.Update(team);
                }
            }

            _tournamentsDbContext.SaveChanges();

            return RedirectToAction("List", "Tournaments");
        }


        [HttpGet()]
        public IActionResult Cancel(int id)
        {
            var tournament = _tournamentsDbContext.Tournaments
                                .Include(t => t.TournamentTeams)
                                .ThenInclude(team => team.Players)
                                .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        [HttpPost()]
        public IActionResult CancelConfirmed(int id)
        {
            var tournament = _tournamentsDbContext.Tournaments
                        .Include(t => t.TournamentTeams)
                        .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            var teamsWithPlayers = _tournamentsDbContext.Teams
                          .Where(t => tournament.TeamIds.Contains(t.TeamId))
                          .Include(t => t.Players)
                          .ToList();

            var allPlayers = teamsWithPlayers.SelectMany(t => t.Players).Distinct().ToList();



            foreach (var player in allPlayers)
            {
                _mail.SendCancellation(tournament, player);
            }

            _tournamentsDbContext.Tournaments.Remove(tournament);
            _tournamentsDbContext.SaveChanges();

            return View("List", getTournaments());
        }
    }
}
