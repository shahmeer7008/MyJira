using Jira_2._0.Models.CustomisedUserModel;

namespace Jira_2._0.Interfaces
{
    public interface IUserRepo
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task ApproveClaimAsync(string userId, string claim);
        Task RejectClaimAsync(string userId);
        Task<List<ApplicationUser>> GetPendingClaimRequestsAsync();
        public List<ApplicationUser> GetAllActiveUsers();
    }

}
