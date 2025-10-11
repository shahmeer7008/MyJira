using Jira_2._0.Interfaces;
using Jira_2._0.Models.CustomisedUserModel;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace Jira_2._0.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IEmailSender _emailSender;

        public EmailNotificationService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendClaimApprovalNotificationAsync(ApplicationUser user, string claim)
        {
            await _emailSender.SendEmailAsync(
                user.Email,
                "Claim Approved",
                $"Your claim for {claim} has been approved.");
        }

        public async Task SendClaimRejectionNotificationAsync(ApplicationUser user)
        {
            await _emailSender.SendEmailAsync(
                user.Email,
                "Claim Rejected",
                "Your claim request has been rejected.");
        }
    }
}
