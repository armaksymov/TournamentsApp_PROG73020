using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MTournamentsApp.Entities
{
    public class Invitation
    {
        public int InvitationId { get; set; }

        [Required(ErrorMessage = "Please enter a player name")]
        public string? PlayerName { get; set; }

        [Required(ErrorMessage = "Please enter a player email")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,}(\.[a-zA-Z]{2,})?$",
            ErrorMessage = "Please enter a valid player email address")]
        public string? PlayerEmail { get; set; }

        [Required(ErrorMessage = "Please select an invitation status")]
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;

        public int PlayerId { get; set; }

        public Player? Player { get; set; }
    }

    public enum InvitationStatus
    {
        InviteNotSent,
        InviteSent,
        RespondedYes,
        RespondedNo
    }
}
