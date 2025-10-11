using Jira_2._0.Models;
using Jira_2._0.Models.DatabaseRepositories;
using Jira_2._0.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using Jira_2._0.Models.CustomisedUserModel;
using Microsoft.AspNetCore.Identity;

namespace Jira_2._0.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    public class AdminController : Controller
    {
        private readonly IProjectRepo _projectRepo;
        private readonly IIssueRepo _IssueRepo;
        private readonly IUserRepo _UserRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminController(IProjectRepo projectRepo, IIssueRepo issueRepo, IUserRepo UserRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _projectRepo = projectRepo;
            _IssueRepo = issueRepo;
            _UserRepo = UserRepo;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult AdminProfile()
        {

            return View();
        }


        public IActionResult AdminDashBoard() {
            ViewBag.ProjectsCompleted = 75;
            ViewBag.IssuesResolved = 53;
            ViewBag.MembersProgress = 44;
            ViewBag.ManagerPerformance = 65;
            return View();
        }

        public IActionResult Projects()
        {
            try
            {
              
                var projects = _projectRepo.GetAllProjects();
                return View(projects ?? new List<ProjectModelWrapper>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Projects action: {ex}");
                return View(new List<ProjectModelWrapper>());
            }
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
        
        public IActionResult CreateProject()
        {
            return View(new ProjectModelWrapper { ProjectData = new ProjectModel() });
        }

        [HttpPost]
        public IActionResult CreateProject(ProjectModelWrapper wrapper)
        {
            if (!ModelState.IsValid)
            {
                return View(wrapper);
            }

            try
            {
                

                int newId = _projectRepo.Insert(wrapper);

                if (newId > 0)
                {
                    TempData["SuccessMessage"] = "Project created successfully!";
                    return RedirectToAction("Projects");
                }

                ModelState.AddModelError("", "Database operation failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\nStack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Operation failed: {ex.Message}");
            }

            return View(wrapper);
        }

        // GET
        public IActionResult EditProject(int id)
        {
            try
            {
                var project = _projectRepo.GetProjectById(id);
                if (project == null)
                {
                    return NotFound();
                }

                return View(project);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching project for edit: {ex.Message}");
                return View("Error");
            }
        }

        // POST
        [HttpPost]
        public IActionResult EditProject(ProjectModelWrapper wrapper)
        {
            Console.WriteLine($"ProjectID from form: {wrapper.ProjectID}");
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
                bool updated = _projectRepo.UpdateProject(wrapper);

                if (updated)
                {
                    TempData["SuccessMessage"] = "Project updated successfully!";
                    return RedirectToAction("Projects");
                }

                ModelState.AddModelError("", "Update failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update error: {ex.Message}");
                ModelState.AddModelError("", ex.Message);
            }

            return View(wrapper);
        }


        [HttpGet] // Change from HttpPost to HttpGet for simplicity
        public IActionResult DeleteProject(int id)
        {
            try
            {
                bool deleted = _projectRepo.DeleteProject(id);
                if (deleted)
                {
                    TempData["SuccessMessage"] = "Project deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete project.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting project: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the project.";
            }

            return RedirectToAction("Projects");
        }

        public IActionResult ViewProject(int id)
        {
            var projectDetails = _projectRepo.GetProjectDetails(id);

            if (projectDetails == null)
                return NotFound("Project not found");

            return View(projectDetails);
        }


        [HttpGet]
        public IActionResult SearchIssues()
        {
            // return the view with no results initially
            return View(new List<IssueModelWrapper>());
        }

        [HttpPost]
        public IActionResult SearchIssues(string projectId, string priority, string status)
        {
            var results = _IssueRepo.SearchIssues(projectId, priority, status);
            Console.WriteLine($"Found {results} issues");
            return View("_SearchResultsPartial", results); // You can render same or separate view
        }

        public async Task<IActionResult> Notifications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // This refreshes the claims in the current session
                await _signInManager.RefreshSignInAsync(user);

                // For debugging purposes, let's check if the user actually has the required properties
                if (user.IsClaimVerified)
                {
                    // Log or add to ViewBag for debugging
                    ViewBag.DebugUserStatus = $"User should be verified. IsClaimVerified: {user.IsClaimVerified}, IsRejected: {user.IsRejected}";
                }
                else
                {
                    ViewBag.DebugUserStatus = $"User is not verified. IsClaimVerified: {user.IsClaimVerified}, IsRejected: {user.IsRejected}";
                }
            }

            return View();
        }



        public IActionResult viewKanban()
        {

            return View();
        } 
        public IActionResult ProjectsReports()
        {

            return View();
        }
        public IActionResult PerformanceReports()
        {

            return View();
        }
        public IActionResult UsersRecord()
        {

            var users = _UserRepo.GetAllActiveUsers();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ClaimRequests()
        {
            var pendingUsers = await _UserRepo.GetPendingClaimRequestsAsync();

            var viewModel = pendingUsers.Select(u => new ClaimRequestViewModel
            {
                UserId = u.Id,
                Username = u.UserName,
                Email = u.Email,
                RequestedClaim = u.RequestedClaim
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> HandleClaim(string userId, string claim, string decision)
        {
            try
            {
                if (decision == "approve")
                {

                    await _UserRepo.ApproveClaimAsync(userId, claim);
                    TempData["Message"] = "Claim approved successfully";
                }
                else if (decision == "reject")
                {
                    await _UserRepo.RejectClaimAsync(userId);
                    TempData["Message"] = "Claim rejected successfully";
                }
            }

            catch (Exception ex) { TempData["Error"] = $"Error processing request: {ex.Message}"; }
            return RedirectToAction("ClaimRequests");
        }

    }
}
