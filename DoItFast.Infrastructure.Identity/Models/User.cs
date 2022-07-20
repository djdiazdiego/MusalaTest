using Microsoft.AspNetCore.Identity;

namespace DoItFast.Infrastructure.Identity.Models
{
    public class User : IdentityUser<Guid>
    {
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token) => RefreshTokens?.Find(x => x.Token == token) != null;
    }
}
