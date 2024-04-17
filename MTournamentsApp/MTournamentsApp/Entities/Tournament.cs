using MTournamentsApp.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTournamentsApp.Entities
{
    public class Tournament
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the tournament name")]
        public string? TournamentName { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Please enter the tournament date")]
        [TournamentDateValidation]
        public DateTime? TournamentDate { get; set; }

        [Required(ErrorMessage = "Please enter the tournament address")]
        public string? Address { get; set; }

        public string? TournamentGameId { get; set; }
        public Game? TournamentGame { get; set; }

        public List<string>? TeamIds {  get; set; }
        public ICollection<Team>? TournamentTeams { get; set; }
    }
}
