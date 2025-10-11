using Jira_2._0.Models.CustomisedUserModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Jira_2._0.Services
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Add custom claims based on user properties
            identity.AddClaim(new Claim("id", user.Id));
            if (!string.IsNullOrEmpty(user.RequestedClaim))
            {
                identity.AddClaim(new Claim("RequestedClaim", user.RequestedClaim));
            }

            // Add IsClaimVerified as a string value - "1" for true, "0" for false
            identity.AddClaim(new Claim("IsClaimVerified", user.IsClaimVerified ? "1" : "0"));

            // Add IsRejected as a string value - "1" for true, "0" for false
            identity.AddClaim(new Claim("IsRejected", user.IsRejected ? "1" : "0"));

            // If the user is verified and not rejected, add their role as a UserType claim
            if (user.IsClaimVerified && !user.IsRejected && !string.IsNullOrEmpty(user.RequestedClaim))
            {
                identity.AddClaim(new Claim("UserType", user.RequestedClaim));
            }

            return identity;
        }
    }


}
