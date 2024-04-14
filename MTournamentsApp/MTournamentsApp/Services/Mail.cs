using MTournamentsApp.Entities;
using System.Net.Mail;
using System.Net;

namespace MTournamentsApp.Services
{
    public class Mail(IHttpContextAccessor httpContextAccessor) : IMail
    {
        public bool SendInvite(Tournament tournament, Invitation recipient)
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
                    $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host.ToUriComponent()}/tournament/{tournament.Id}/invitations/{recipient.InvitationId}";

                // Create mail message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = $"You have been invited to the \"{tournament.TournamentName}\" tournament!",
                    Body = $"",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipient.PlayerEmail);
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
