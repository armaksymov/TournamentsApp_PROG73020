using System.ComponentModel.DataAnnotations;

namespace MTournamentsApp.Entities
{
    public class Team
    {
        public string? TeamId { get; set; }

        [Required(ErrorMessage = "Please provide the team's name")]
        public string? TeamName { get; set; }

        public string? TeamDescription { get; set;}

        [Required(ErrorMessage = "Please select the team's game")]
        public string? MainTeamGameId { get; set; }
        public Game? MainTeamGame { get; set; }

        public List<int>? PlayerIds { get; set; }
        public ICollection<Player>? Players { get; set; }

        public List<int>? TournamentIds { get; set; }
        public ICollection<Tournament>? Tournaments { get; set; }
    }
}
