using DoItFast.Application.Features.Dtos.Email;
using DoItFast.Application.Services.Interfaces;
using DoItFast.Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DoItFast.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailSettings"></param>
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        /// <inheritdoc />
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = mailRequest.Subject ?? ""
            };

            foreach (var toEmail in mailRequest.ToEmail)
            {
                email.To.Add(MailboxAddress.Parse(toEmail));
            }

            if(mailRequest.FromEmail != null && mailRequest.FromEmail.Any())
            {
                foreach (var fromEmail in mailRequest.FromEmail)
                {
                    email.To.Add(MailboxAddress.Parse(fromEmail));
                }
            }

            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body ?? "";
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
