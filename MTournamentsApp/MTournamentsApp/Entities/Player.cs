using MTournamentsApp.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTournamentsApp.Entities
{
    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide the player's first name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please provide the player's last name")]
        public string? LastName { get; set; }

        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Please provide the player's first name")]
        [BirthDateValidation]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please provide the player's email address")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,}(\.[a-zA-Z]{2,})?$",
        ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        public int? Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;

                if (today.Month < DateOfBirth.Month || (today.Month == DateOfBirth.Month && today.Day < DateOfBirth.Day))
                {
                    age--;
                }

                return age;
            }
        }

        [Required(ErrorMessage = "Please provide the player's role")]
        public string? PlayerRoleId { get; set; }
        public PlayerRole? Role { get; set; }

        public string? TeamId { get; set; }
        public Team? Team { get; set; }

        public List<Invitation>? Invitations { get; set; } = null;
    }
}
