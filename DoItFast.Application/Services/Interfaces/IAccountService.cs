using DoItFast.Application.Features.Dtos.Account;
using DoItFast.Application.Wrappers;

namespace DoItFast.Application.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        /// <summary>
        /// Confirm user email.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        /// <summary>
        /// Forgot user password.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
    }
}
