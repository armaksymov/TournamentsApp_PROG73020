using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTournamentsApp.Entities;
using MTournamentsApp.Models;
using System.IO;

namespace MTournamentsApp.Controllers
{
    public class TournamentsController : Controller
    {
        private TournamentsDbContext _tournamentsDbContext;

        public TournamentsController(TournamentsDbContext tournamentsDbContext)
        {
            _tournamentsDbContext = tournamentsDbContext;
        }

        public IActionResult List()
        {
            return View(getTournaments());
        }

        [HttpGet()]
        public IActionResult Add()
        {
            List<Game> games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
            List<Team> teams = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();

            return View(new TournamentViewModel() { Tournament = new Tournament(), GamesList = games, TeamsList = teams });
        }

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
                tournament.TournamentTeams = _tournamentsDbContext.Teams.Where(t => tournament.TeamIds.Contains(t.TeamId)).OrderBy(t => t.TeamId).ToList();
            }

            return tournaments;
        }
    }
}
