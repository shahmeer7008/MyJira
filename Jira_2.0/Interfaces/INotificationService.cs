using Jira_2._0.Models.CustomisedUserModel;

namespace Jira_2._0.Interfaces
{
    public interface INotificationService
    {
        Task SendClaimApprovalNotificationAsync(ApplicationUser user, string claim);
        Task SendClaimRejectionNotificationAsync(ApplicationUser user);
    }
}
