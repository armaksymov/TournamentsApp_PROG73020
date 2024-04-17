using MTournamentsApp.Entities;

namespace MTournamentsApp.Models
{
    public class TournamentRequest
    {
        public Address? Address { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
