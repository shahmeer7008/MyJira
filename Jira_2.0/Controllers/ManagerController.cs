using Jira_2._0.Models;
using Jira_2._0.Models.DatabaseRepositories;
using Jira_2._0.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Jira_2._0.Models.CustomisedUserModel;
namespace Jira_2._0.Controllers
{
    [Authorize(Policy = "RequireProjectManager")]
    public class ManagerController : Controller
    {
        private readonly IIssueRepo _IssueRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ManagerController(IIssueRepo issueRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _IssueRepo = issueRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult ManagerDashboard()
        {
            ViewBag.ProjectCompleted = 75;
            ViewBag.IssuesResolved = 53;
            ViewBag.AddedIssues = 44;
            ViewBag.TeamPerformance = 65;
            ViewBag.IssuesResolvedWeekly = new List<int> { 12, 19, 8, 15, 12, 5, 18 };
            ViewBag.TeamPerformanceData = new List<int> { 90, 80, 85, 70, 75 };
            ViewBag.TeamMembers = new List<string> { "Alex", "Sarah", "Michael", "Emma", "David" };
            return View();

        }
        public IActionResult ManagerProfile()
        {
            return View();
        }
        public async Task<IActionResult> Issues()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var manager = await _userManager.FindByIdAsync(userId);
                string name = manager.Name;
                var projects = _IssueRepo.GetAllIssuesForManager(name);
                return View(projects ?? new List<IssueModelWrapper>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Issues action: {ex}");
                return View(new List<IssueModelWrapper>());
            }
        }
        public IActionResult ProjectDetails()
        {
            return View();
        }
        public IActionResult AddIssue()
        {
          
            return View(new IssueModelWrapper { IssueData = new IssueModel() });
        }
        [HttpPost]
        public IActionResult AddIssue(IssueModelWrapper wrapper)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Field: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(wrapper);
            }

            try
            {


                int newId = _IssueRepo.Insert(wrapper);

                if (newId > 0)
                {
                    TempData["SuccessMessage"] = "Issue Added successfully!";
                    return RedirectToAction("Issues");
                }

                ModelState.AddModelError("", "Database operation failed");
                Console.WriteLine("Hello1");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\nStack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Operation failed: {ex.Message}");
            }

            return View(wrapper);
        }
        public IActionResult EditIssue(int id )
        {
            
            try
            {
                Console.WriteLine("bi");
                var wrapper = _IssueRepo.GetIssueById(id);
                if (wrapper == null || wrapper.IssueData == null)
                {
                    Console.WriteLine("si");
                    return NotFound();
                }
                Console.WriteLine("gi");
                return View(wrapper);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching issue for edit: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult EditIssue(IssueModelWrapper wrapper )
        {
            Console.WriteLine("Hellog");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid. Errors:");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Field: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }

                return View(wrapper);
            }


            try
            {
                Console.WriteLine("Hi");
                bool updated = _IssueRepo.UpdateIssue(wrapper);
                Console.WriteLine(updated);
                if (updated)
                {

                    TempData["SuccessMessage"] = "Issue updated successfully!";
                    return RedirectToAction("Issues");
                }
                Console.WriteLine("wow");
                ModelState.AddModelError("", "Update failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update error: {ex.Message}");
                ModelState.AddModelError("", ex.Message);
            }
            Console.WriteLine("nice ");
            return View(wrapper);
        }
        [HttpGet]
        public IActionResult DeleteIssue(int id)
        {
            Console.WriteLine(id);
            try
            {
                Console.WriteLine("hehe");
                bool deleted = _IssueRepo.DeleteIssue(id);
                if (deleted)
                {
                    TempData["SuccessMessage"] = "Issue deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete issue.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting issue: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the issue.";
            }

            return RedirectToAction("Issues");
        }
        public IActionResult KanbanBoard()
        {
            return View();
        }

        public async Task<IActionResult> Notifications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // This refreshes the claims in the current session
                await _signInManager.RefreshSignInAsync(user);
            }

            return View();
        }

        public IActionResult PerformanceReport()
        {
            return View();
        }
        public IActionResult ProjectReport()
        {
            return View();
        } 
        public IActionResult viewIssue()
        {
            return View();
        }


    private void LogValidationErrors()
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Validation Errors Summary:");
            Console.WriteLine($"Total Errors: {ModelState.ErrorCount}");

            foreach (var key in ModelState.Keys)
            {
                var entry = ModelState[key];
                if (entry.Errors.Count > 0)
                {
                    Console.WriteLine($"Field: {key}");
                    foreach (var error in entry.Errors)
                    {
                        // Log both the error message and exception if it exists
                        Console.WriteLine($"- Error: {error.ErrorMessage}");
                        if (error.Exception != null)
                        {
                            Console.WriteLine($"  Exception: {error.Exception.Message}");
                            Console.WriteLine($"  Stack Trace: {error.Exception.StackTrace}");
                        }
                    }
                }
            }

            // Also log the raw values that caused validation issues
            Console.WriteLine("Problematic Values:");
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].AttemptedValue != null)
                {
                    Console.WriteLine($"{key}: '{ModelState[key].AttemptedValue}'");
                }
            }
        }
        else
        {
            Console.WriteLine("No validation errors detected.");
        }
    }
}

    }