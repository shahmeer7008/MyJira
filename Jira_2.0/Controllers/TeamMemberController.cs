using Jira_2._0.Interfaces;
using Jira_2._0.Models.CustomisedUserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jira_2._0.Controllers
{
    [Authorize(Policy = "RequireTeamMember")]
    public class TeamMemberController : Controller
    {
        private readonly IIssueRepo _IssueRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamMemberController(IIssueRepo issueRepo, UserManager<ApplicationUser> userManager)
        {
            _IssueRepo = issueRepo;
            _userManager = userManager;

        }
        public IActionResult MemberDashboard()
        {
            ViewBag.Contribution = 75;
            ViewBag.IssuesResolved = 53;
            ViewBag.PriorityPending = 44;
            ViewBag.ReportIssues = 65;
            return View();
        }
        public IActionResult MemberProfile()
        {
            return View();
        }
      
        [HttpGet]
        public async Task<IActionResult> AssignedIssues()
        {
            Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");

            var userName = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("hehe");
                return Unauthorized(); // Or redirect to login
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                Console.WriteLine("hiiii");
                return Unauthorized(); // User not found
            }

            var myuser = await _userManager.GetUserAsync(User);
            var fullName = myuser?.Name;
            Console.WriteLine(fullName);
                var issues = await _IssueRepo.GetAssignedIssuesForUserAsync(fullName);
            return View(issues);
        }

        [HttpPost]
        public async Task<IActionResult> AssignedIssues(int issueId, string status, string Title, string priority,int ProjectID)
        {
            string id=User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _IssueRepo.UpdateIssueStatusAndPriorityAsync(issueId, status, priority,Title,ProjectID,id);
            return RedirectToAction("AssignedIssues");
        }
        //public IActionResult KanbanBoard()
        //{
        //    return View();
        //}
        public IActionResult Notifications()
        {
            return View();
        } 
        public IActionResult ProjectDetails()
        {
            return View();
        }
        //public IActionResult ReportIssues()
        //{
        //    return View();
        //} 
        public async Task<IActionResult> ResolvedIssues()
        {
            string userId = _userManager.GetUserId(User);
            // Or use from session/claims
            var issues = await _IssueRepo.GetResolvedIssuesByUserAsync(userId);
            Console.WriteLine("yahha");
            return View(issues);
        }



    }
}
