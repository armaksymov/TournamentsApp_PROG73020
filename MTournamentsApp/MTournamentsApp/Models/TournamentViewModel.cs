using MTournamentsApp.Entities;

namespace MTournamentsApp.Models
{
    public class TournamentViewModel
    {
        public ICollection<Game>? GamesList { get; set; }
        public ICollection<Team>? TeamsList { get; set; }
        public Tournament? Tournament { get; set; }
        public List<string>? SelectedTeamIds { get; set; }
    }
}
