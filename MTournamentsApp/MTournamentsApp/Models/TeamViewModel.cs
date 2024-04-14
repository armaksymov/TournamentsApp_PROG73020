using MTournamentsApp.Entities;

namespace MTournamentsApp.Models
{
    public class TeamViewModel
    {
        public ICollection<Game>? GamesList { get; set; }
        public ICollection<Player>? PlayersList { get; set; }

        public Team? Team { get; set; }
    }
}
