    using Microsoft.AspNetCore.Identity.UI.Services;
    using System.Threading.Tasks;

namespace Jira_2._0.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just log to console or skip actual sending
            Console.WriteLine($"Email to {email}: {subject}");
            return Task.CompletedTask;
        }
    }

}
