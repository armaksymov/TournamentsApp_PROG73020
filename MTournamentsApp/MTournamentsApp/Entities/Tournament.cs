using MTournamentsApp.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTournamentsApp.Entities
{
    public class Tournament
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Tournament Name")]
        public string? TournamentName { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Please enter the Tournament Date")]
        [TournamentDateValidation]
        public DateTime? TournamentDate {  get; set; }

        public int? AddressID { get; set; }
        public Address? Address { get; set; }

        public string? TournamentGameId { get; set; }
        public Game? TournamentGame { get; set; }

        public ICollection<string>? TeamIds {  get; set; }
        public ICollection<Team>? TournamentTeams { get; set; }
    }
}
