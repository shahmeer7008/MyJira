using Microsoft.AspNetCore.Identity;




namespace Jira_2._0.Models.CustomisedUserModel
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsClaimVerified { get; set; } = false;
        public string? RequestedClaim { get; set; }
        public bool IsRejected { get; set; } = false;

        public string Name { get; set; }

    }
}
