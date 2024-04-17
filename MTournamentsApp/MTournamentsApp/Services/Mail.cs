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
                const string fromAddress = "mtournamentz@outlook.com";
                const string fromAddressPassword = "bYRc2I/71]5y";

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

        public bool SendCancellation(Tournament tournament, Player recipient)
        {
            try
            {
                const string fromAddress = "mtournamentz@outlook.com";
                const string fromAddressPassword = "bYRc2I/71]5y";

                var smtpClient = new SmtpClient("smtp-mail.outlook.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, fromAddressPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                // Create mail message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = $"Cancellation of the \"{tournament.TournamentName}\" Tournament",
                    Body = $"<p>Dear {recipient.FirstName},</p>" +
                   $"<p>We regret to inform you that the \"{tournament.TournamentName}\" tournament scheduled for {tournament.TournamentDate?.ToString("dddd, MMMM dd, yyyy")} has been cancelled due to unforeseen circumstances.</p>" +
                   $"<p>We understand this may be disappointing and apologize for any inconvenience this may cause. We value your participation and look forward to seeing you at future tournaments.</p>" +
                   $"<p>If you have any questions or need further information, please feel free to contact us at any time.</p>" +
                   $"<p>Thank you for your understanding.</p>" +
                   $"<p>Sincerely,</p>" +
                   $"<p>The MTournamentZ Team</p>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipient.Email);
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
