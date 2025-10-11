using Jira_2._0.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Jira_2._0.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Send(string message, string userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
            else
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);

            return Ok();
        }
    }
}
