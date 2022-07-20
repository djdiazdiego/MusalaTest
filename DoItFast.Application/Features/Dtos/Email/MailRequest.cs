using Microsoft.AspNetCore.Http;

namespace DoItFast.Application.Features.Dtos.Email
{
    public class MailRequest
    {
        /// <summary>
        /// Destination
        /// </summary>
        public List<string> ToEmail { get; set; }
        /// <summary>
        /// Sender
        /// </summary>
        public List<string> FromEmail { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Attachments
        /// </summary>
        public List<IFormFile> Attachments { get; set; }
    }
}
