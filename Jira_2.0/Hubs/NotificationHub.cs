using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Jira_2._0.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        // Enum to track sender role
        private enum SenderRole
        {
            Admin,
            ProjectManager,
            TeamMember
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            foreach (var claim in Context.User.Claims)
            {
                _logger.LogInformation($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            if (!string.IsNullOrEmpty(userId))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, userId); // or store in map
            }
            _logger.LogInformation($"Client connected: {Context.ConnectionId}, User: {Context.UserIdentifier}");

            await base.OnConnectedAsync();
        }
       
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}, User: {Context.UserIdentifier}");
            await base.OnDisconnectedAsync(exception);
        }

        // Helper method to send notification with role-specific message ID prefix
        private async Task SendNotification(string recipientId, string message, SenderRole role,
            bool isReply = false, string originalMessageId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(recipientId))
                {
                    throw new ArgumentException("Recipient ID cannot be null or empty");
                }

                var senderId = Context.UserIdentifier;
                var senderName = Context.User?.Identity?.Name ?? "Unknown";
                var timestamp = DateTime.Now;

                _logger.LogInformation($"Sending notification from {senderId} to {recipientId}");

                // Generate appropriate message ID based on role and whether it's a reply
                string messageIdPrefix = role switch
                {
                    SenderRole.Admin => "AD",
                    SenderRole.ProjectManager => "PM",
                    SenderRole.TeamMember => "TM",
                    _ => "GN" // Generic as fallback
                };

                string messageId = isReply
                    ? $"{messageIdPrefix}-REPLY-{Guid.NewGuid()}"
                    : $"{messageIdPrefix}-{Guid.NewGuid()}";

                await Clients.User(recipientId).SendAsync("ReceiveNotification", new
                {
                    MessageId = messageId,
                    Sender = senderName,
                    Message = message,
                    Timestamp = timestamp,
                    IsReply = isReply,
                    OriginalSenderId = senderId,
                    OriginalMessageId = originalMessageId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
                throw; // Re-throw to propagate to client
            }
        }

        // Admin sending methods
        public async Task SendToProjectManager(string projectManagerId, string message, string originalMessageId = "")
        {
            _logger.LogInformation($"Sending to user ID: {projectManagerId}");

            await SendNotification(projectManagerId, message, SenderRole.Admin, false, originalMessageId);
        }

        public async Task SendToTeamMember(string teamMemberId, string message, string originalMessageId = "")
        {
            await SendNotification(teamMemberId, message, SenderRole.Admin, false, originalMessageId);
        }

        public async Task ReplyToProjectManager(string projectManagerId, string message, string originalMessageId)
        {
            if (string.IsNullOrEmpty(originalMessageId))
            {
                throw new ArgumentException("Original message ID is required for replies");
            }
            await SendNotification(projectManagerId, message, SenderRole.Admin, true, originalMessageId);
        }

        public async Task ReplyToTeamMember(string teamMemberId, string message, string originalMessageId)
        {
            if (string.IsNullOrEmpty(originalMessageId))
            {
                throw new ArgumentException("Original message ID is required for replies");
            }
            await SendNotification(teamMemberId, message, SenderRole.Admin, true, originalMessageId);
        }

        // Project Manager sending methods
        public async Task SendToAdminFromProjectManager(string adminId, string message, string originalMessageId = "")
        {
            await SendNotification(adminId, message, SenderRole.ProjectManager, false, originalMessageId);
        }

        public async Task ReplyToAdminFromProjectManager(string adminId, string message, string originalMessageId)
        {
            if (string.IsNullOrEmpty(originalMessageId))
            {
                throw new ArgumentException("Original message ID is required for replies");
            }
            await SendNotification(adminId, message, SenderRole.ProjectManager, true, originalMessageId);
        }

        // Team Member sending methods
        public async Task SendToAdminFromTeamMember(string adminId, string message, string originalMessageId = "")
        {
            await SendNotification(adminId, message, SenderRole.TeamMember, false, originalMessageId);
        }

        public async Task ReplyToAdminFromTeamMember(string adminId, string message, string originalMessageId)
        {
            if (string.IsNullOrEmpty(originalMessageId))
            {
                throw new ArgumentException("Original message ID is required for replies");
            }
            await SendNotification(adminId, message, SenderRole.TeamMember, true, originalMessageId);
        }
    }
}