using DoItFast.Application.Features.Dtos.Email;

namespace DoItFast.Application.Services.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
