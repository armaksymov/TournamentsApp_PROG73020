using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTournamentsApp.Entities;
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
        private List<Tournament> getTournaments()
        {
            List<Tournament> tournaments = _tournamentsDbContext.Tournaments.Include(t => t.Address).Include(t => t.TournamentGame).ToList();

            foreach (var tournament in tournaments)
            {
                tournament.TournamentTeams = _tournamentsDbContext.Teams.Where(t => tournament.TeamIds.Contains(t.TeamId)).OrderBy(t => t.TeamId).ToList();
            }

            return tournaments;
        }
    }
}
