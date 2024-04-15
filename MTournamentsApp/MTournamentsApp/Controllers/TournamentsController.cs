using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MTournamentsApp.Entities;
using MTournamentsApp.Models;
using MTournamentsApp.Services;
using System.IO;

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
        private List<Tournament> getTournaments()
        {
            List<Tournament> tournaments = _tournamentsDbContext.Tournaments.Include(t => t.Address).Include(t => t.TournamentGame).ToList();

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

        [HttpPost()]
        public IActionResult Add(TournamentViewModel tvm, List<string> SelectedTeamIds)
        {
            if (!ModelState.IsValid || SelectedTeamIds.Count < 2)
            {
                if (SelectedTeamIds.Count < 2)
                {
                    ModelState.AddModelError("SelectedTeamIds", "Please select at least two teams for the tournament.");
                }
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
                if(team.TournamentIds.IsNullOrEmpty())
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

        [HttpGet()]
        public IActionResult Edit(int id)
        {
            List<Game> games = _tournamentsDbContext.Games.OrderBy(g => g.GameName).ToList();
            List<Team> teams = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();
            Tournament tournament = _tournamentsDbContext.Tournaments.Include(t => t.Address).Include(t => t.TournamentTeams).Where(t => t.Id == id).FirstOrDefault();

            return View(new TournamentViewModel() { Tournament = tournament, GamesList = games, TeamsList = teams });
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

            return View("List");
        }
    }
}
