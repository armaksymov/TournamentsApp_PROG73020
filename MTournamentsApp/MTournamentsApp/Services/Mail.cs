using MTournamentsApp.Entities;
using System.Net.Mail;
using System.Net;

namespace MTournamentsApp.Services
{
    public class Mail(IHttpContextAccessor httpContextAccessor) : IMail
    {
        public bool SendInvite(Tournament tournament, Player player)
        {
            try
            {
                const string fromAddress = "";
                const string fromAddressPassword = "";

                var smtpClient = new SmtpClient("smtp-mail.outlook.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, fromAddressPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                var invitationUrl =
                    $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host.ToUriComponent()}/tournament/{tournament.Id}/invitations/{player.Id}";

                // Create mail message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = $"You have been invited to the \"{tournament.TournamentName}\" tournament!",
                    Body = $"" +
                    $"<h1>Hello {player.FirstName}:</h1>" +
                    $"<p>You have been invited to the \"{tournament.TournamentName}\" for {tournament.TournamentGame} on {tournament.TournamentDate}!</p>" +
                    $"<p>We would be thrilled to have you so please <a href=\"{invitationUrl}\">let us know</a> if you can as soon as possible!</p>" +
                           $"<p>Sincerely,</p>" +
                           $"<p>MTournamentZ</p>",
                    IsBodyHtml = true
                };

                if (player.Email != null)
                {
                    mailMessage.To.Add(player.Email);
                    smtpClient.Send(mailMessage);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
