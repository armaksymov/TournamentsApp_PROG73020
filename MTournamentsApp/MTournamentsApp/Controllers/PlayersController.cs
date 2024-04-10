using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTournamentsApp.Entities;
using MTournamentsApp.Models;

namespace MTournamentsApp.Controllers
{
    public class PlayersController : Controller
    {
        private TournamentsDbContext _tournamentsDbContext;

        public PlayersController(TournamentsDbContext tournamentsDbContext)
        {
            _tournamentsDbContext = tournamentsDbContext;
        }

        public IActionResult List()
        {
            return View(GetPlayers());
        }
        private List<Player> GetPlayers()
        {
            List<Player> players = _tournamentsDbContext.Players.Include(p => p.Role).Include(p => p.Team).ToList();
            return players;
        }

        [HttpGet()]
        public IActionResult Add()
        {
            List<PlayerRole> playerRoles = _tournamentsDbContext.PlayerRoles.OrderBy(pr => pr.PlayerRoleName).ToList();
            List<Team> teams = _tournamentsDbContext.Teams.OrderBy(p => p.TeamName).ToList();

            return View(new PlayerViewModel() { Player = new Player(), RolesList = playerRoles, TeamsList = teams });
        }

        [HttpPost()]
        public IActionResult Add(PlayerViewModel p)
        {
            if (ModelState.IsValid)
            {
                _tournamentsDbContext.Players.Add(p.Player);
                Team? team = _tournamentsDbContext.Teams.Include(t => t.Players).Where(t => t.TeamId == p.Player.TeamId).FirstOrDefault();

                if (team != null)
                {
                    team.PlayerIds.Add(p.Player.Id);
                    team.Players.Add(p.Player);
                }

                _tournamentsDbContext.Teams.Update(team);//Where(t => t.TeamId == p.Player.TeamId);

                _tournamentsDbContext.SaveChanges();

                return RedirectToAction("List", "Players");
            }
            else
            {
                List<PlayerRole> playerRoles = _tournamentsDbContext.PlayerRoles.OrderBy(pr => pr.PlayerRoleName).ToList();
                List<Team> teams = _tournamentsDbContext.Teams.OrderBy(p => p.TeamName).ToList();

                return View(new PlayerViewModel() { Player = new Player(), RolesList = playerRoles, TeamsList = teams });
            }
        }
    }
}
