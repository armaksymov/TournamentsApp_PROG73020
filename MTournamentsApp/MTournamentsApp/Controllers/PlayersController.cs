using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTournamentsApp.Entities;

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
    }
}
