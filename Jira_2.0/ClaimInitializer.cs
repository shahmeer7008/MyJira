using Microsoft.AspNetCore.Identity;
using Jira_2._0.Models.CustomisedUserModel;
using System.Security.Claims;

namespace Jira_2._0
{
    public static class ClaimInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminEmail = "bsef22m501@pucit.edu.pk";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Name="Shahmeer",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    RequestedClaim = "Admin",
                    IsClaimVerified = true,
                    IsRejected = false
                };

                var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(adminUser, new Claim("Permission", "Admin"));
                }
            }
            else
            {
                var claims = await userManager.GetClaimsAsync(adminUser);
                if (!claims.Any(c => c.Type == "Permission" && c.Value == "Admin"))
                {
                    await userManager.AddClaimAsync(adminUser, new Claim("Permission", "Admin"));
                }
            }
        }
    }
}
