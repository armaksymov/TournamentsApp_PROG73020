﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MTournamentsApp.Entities;
using MTournamentsApp.Models;

namespace MTournamentsApp.Controllers
{
    public class MembersController : Controller
    {
        private TournamentsDbContext _tournamentsDbContext;

        public MembersController(TournamentsDbContext tournamentsDbContext)
        {
            _tournamentsDbContext = tournamentsDbContext;
        }

        public IActionResult List()
        {
            return View(GetMembers());
        }
        private List<Player> GetMembers()
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
                    if (team.PlayerIds.IsNullOrEmpty())
                    {
                        team.PlayerIds = new List<int>();
                    }
                    if (team.Players.IsNullOrEmpty())
                    {
                        team.Players = new List<Player>();
                    }
                    team.PlayerIds.Add(p.Player.Id);
                    team.Players.Add(p.Player);
                }

                if (team != null)
                    _tournamentsDbContext.Teams.Update(team);//Where(t => t.TeamId == p.Player.TeamId);

                _tournamentsDbContext.SaveChanges();

                return RedirectToAction("List", "Members");
            }
            else
            {
                List<PlayerRole> playerRoles = _tournamentsDbContext.PlayerRoles.OrderBy(pr => pr.PlayerRoleName).ToList();
                List<Team> teams = _tournamentsDbContext.Teams.OrderBy(p => p.TeamName).ToList();

                return View(new PlayerViewModel() { Player = new Player(), RolesList = playerRoles, TeamsList = teams });
            }
        }

        [HttpGet()]
        public IActionResult Edit(int id)
        {
            List<PlayerRole> playerRoles = _tournamentsDbContext.PlayerRoles.OrderBy(pr => pr.PlayerRoleName).ToList();
            List<Team> teams = _tournamentsDbContext.Teams.OrderBy(t => t.TeamName).ToList();
            Player? player = _tournamentsDbContext.Players.Include(p => p.Team).Include(p => p.Role).Where(p => p.Id == id).FirstOrDefault();

            if (player != null)
            {
                return View(new PlayerViewModel() { Player = player, RolesList = playerRoles, TeamsList = teams });
            }
            else
            {
                return RedirectToAction("List", "Members");
            }
        }

        [HttpPost()]
        public IActionResult Edit(PlayerViewModel p)
        {
            if (ModelState.IsValid)
            {
                _tournamentsDbContext.Players.Update(p.Player);
                _tournamentsDbContext.SaveChanges();

                return RedirectToAction("List", "Members");
            }
            else
            {
                p.TeamsList = _tournamentsDbContext.Teams.ToList();
                p.RolesList = _tournamentsDbContext.PlayerRoles.ToList();
                return View(p);
            }
        }

        [HttpGet()]
        public IActionResult Delete(int id)
        {
            var player = _tournamentsDbContext.Players.Include(p => p.Role).Include(p => p.Team).Where(p => p.Id == id).FirstOrDefault();
            if (player != null)
            {
                return View(player);
            }
            else
            {
                return RedirectToAction("List", "Members");
            }
        }

        [HttpPost()]
        public IActionResult Delete(Player p)
        {
            _tournamentsDbContext.Players.Remove(p);

            Team? team = _tournamentsDbContext.Teams.Include(t => t.Players).Where(t => t.TeamId == p.TeamId).FirstOrDefault();

            if (team != null)
            {
                team.PlayerIds?.Remove(p.Id);
                foreach (Player player in team.Players)
                {
                    if (player.Id == p.Id)
                    {
                        team.Players.Remove(player);
                    }
                }
                _tournamentsDbContext.Teams.Update(team);
            }

            _tournamentsDbContext.SaveChanges();

            return RedirectToAction("List", "Members");
        }
    }
}