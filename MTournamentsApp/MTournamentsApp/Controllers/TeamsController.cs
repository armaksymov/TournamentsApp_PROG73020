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

        [HttpPost()]
        public IActionResult Delete(Team t)
        {
            _tournamentsDbContext.Teams.Remove(t);
            _tournamentsDbContext.SaveChanges();

            return RedirectToAction("List", "Teams");
        }

        [HttpGet()]
        public IActionResult Players(string id)
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
