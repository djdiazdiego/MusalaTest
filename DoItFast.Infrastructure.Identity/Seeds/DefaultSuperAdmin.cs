using DoItFast.Infrastructure.Identity.Enums;
using DoItFast.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace DoItFast.Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            //Seed Default User
            var superAdmin = new User
            {
                UserName = "dayron",
                Email = "dj.diazdiego@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != superAdmin.Id))
            {
                var user = await userManager.FindByEmailAsync(superAdmin.Email);
                if (user == null)
                {
                    var roles = new string[] {
                        Roles.Basic.ToString(),
                        Roles.Moderator.ToString(),
                        Roles.Admin.ToString(),
                        Roles.SuperAdmin.ToString()
                    };

                    await userManager.CreateAsync(superAdmin, "Moster16@").ContinueWith(async task =>
                        await userManager.AddToRolesAsync(superAdmin, roles));
                }
            }
        }
    }
}
