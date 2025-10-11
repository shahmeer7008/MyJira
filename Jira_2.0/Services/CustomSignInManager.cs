namespace Jira_2._0.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using System.Security.Claims;
    using Jira_2._0.Models.CustomisedUserModel;
    using Microsoft.AspNetCore.Authentication;

    public class CustomSignInManager : SignInManager<ApplicationUser>
    {
        public CustomSignInManager(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user)
        {
            var principal = await base.CreateUserPrincipalAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            identity.AddClaim(new Claim("RequestedClaim", user.RequestedClaim ?? ""));
            identity.AddClaim(new Claim("IsClaimVerified", user.IsClaimVerified.ToString()));
            identity.AddClaim(new Claim("IsRejected", user.IsRejected.ToString()));

            return principal;
        }
    }

}
