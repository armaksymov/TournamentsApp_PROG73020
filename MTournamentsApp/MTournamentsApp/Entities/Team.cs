namespace MTournamentsApp.Entities
{
    public class Team
    {
        public string? TeamId { get; set; }

        public string? TeamName { get; set; }

        public string? TeamDescription { get; set;}

        public string? MainTeamGameId { get; set; }
        public Game? MainTeamGame { get; set; }

        public ICollection<int>? PlayerIds { get; set; }
        public ICollection<Player>? Players { get; set; }

        public ICollection<int>? TournamentIds { get; set; }
        public ICollection<Tournament>? Tournaments { get; set; }
    }
}
