using DoItFast.Infrastructure.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace DoItFast.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        /// <summary>
        /// Seed Roles
        /// </summary>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            foreach (var role in Enum.GetValues<Roles>())
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
            }
        }
    }
}
