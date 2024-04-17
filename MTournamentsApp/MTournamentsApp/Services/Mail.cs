using MTournamentsApp.Entities;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace MTournamentsApp.Services
{
    public class Mail(IHttpContextAccessor httpContextAccessor) : IMail
    {
        const string fromAddress = "mtournamentz@outlook.com";
        const string fromAddressPassword = "bYRc2I/71]5y";
        public bool SendInvite(Tournament tournament, Invitation recipient)
        {
            var invitationUrl =
                $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host.ToUriComponent()}/Tournaments/{tournament.Id}/Invitations/{recipient.InvitationId}";

            try
            {
                var smtpClient = new SmtpClient("smtp-mail.outlook.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, fromAddressPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = $"You have been invited to the \"{tournament.TournamentName}\" tournament!",
                    Body = $"" +
                    $"<h1>Hello {recipient.PlayerName}:</h1>" +
                    $"<p>You have been invited to the \"{tournament.TournamentName}\" at {tournament.Address.ToString()} on {tournament.TournamentDate}!</p>" +
                    $"<p>We would be thrilled to have you so please <a href=\"{invitationUrl}\">let us know</a> if you can as soon as possible!</p>" +
                    $"<p>Sincerely,</p>" +
                    $"<p>The MTournamentZ Team</p>",
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
                var smtpClient = new SmtpClient("smtp-mail.outlook.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, fromAddressPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = $"Cancellation of the \"{tournament.TournamentName}\" Tournament",
                    Body = $"<p>Dear {recipient.FirstName},</p>" +
                   $"<p>We regret to inform you that the \"{tournament.TournamentName}\" tournament scheduled for {tournament.TournamentDate?.ToString("dddd, MMMM dd, yyyy")} has been cancelled due to unforeseen circumstances.</p>" +
                   $"<p>We understand this may be disappointing and apologize for any inconvenience this may cause. We value your participation and look forward to seeing you at future tournaments.</p>" +
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

        public bool SendTeamKick(Tournament tournament, Player recipient)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp-mail.outlook.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, fromAddressPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = $"Team Disqualification from the \"{tournament.TournamentName}\" Tournament",
                    Body = $"<p>Dear {recipient.FirstName},</p>" +
                    $"<p>We regret to inform you that your team has been removed from the \"{tournament.TournamentName}\" tournament scheduled for {tournament.TournamentDate?.ToString("dddd, MMMM dd, yyyy")} due to non-compliance with the tournament regulations.</p>" +
                    $"<p>This decision was made to maintain the integrity and fairness of the competition. We appreciate your past participation and hope this situation will be a learning opportunity.</p>" +
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
