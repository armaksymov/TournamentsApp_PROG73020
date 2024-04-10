using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTournamentsApp.Entities;
using System.IO;

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
    }
}
