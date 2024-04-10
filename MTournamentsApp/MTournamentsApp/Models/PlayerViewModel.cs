using MTournamentsApp.Entities;

namespace MTournamentsApp.Models
{
    public class PlayerViewModel
    {
        public ICollection<PlayerRole>? RolesList { get; set; }
        public ICollection<Team>? TeamsList { get; set; }

        public Player? Player { get; set; }
    }
}
    