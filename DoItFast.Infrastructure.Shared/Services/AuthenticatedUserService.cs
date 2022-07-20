using DoItFast.Infrastructure.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DoItFast.Infrastructure.Shared.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            UserId = Guid.TryParse(user?.FindFirstValue(JwtRegisteredClaimNames.Jti), out Guid userId) ? userId : null;
            UserName = user?.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";
            Email = user?.FindFirstValue(JwtRegisteredClaimNames.Email) ?? "";
            Ip = user?.FindFirstValue("ip") ?? "";
        }
        public Guid? UserId { get; }
        public string UserName { get; }
        public string Email { get; }
        public string Ip { get; }
    }
}
