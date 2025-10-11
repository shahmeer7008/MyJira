using Microsoft.AspNetCore.SignalR;

namespace Jira_2._0.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.FindFirst("id")?.Value;
           

            if ( !string.IsNullOrEmpty(userId))
            {
                return $"{userId}"; // e.g., PM-5, TM-23, AD-1
            }

            return null;
        }
    }


}
