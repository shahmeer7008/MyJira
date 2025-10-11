using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Jira_2._0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,ProjectManager")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("admins")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdmins()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            return Ok(users.Select(u => new { u.Id, u.UserName }));
        }

        [HttpGet("projectmanagers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectManagers()
        {
            var users = await _userManager.GetUsersInRoleAsync("ProjectManager");
            return Ok(users.Select(u => new { u.Id, u.UserName }));
        }

        [HttpGet("teammembers")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> GetTeamMembers()
        {
            var users = await _userManager.GetUsersInRoleAsync("TeamMember");
            return Ok(users.Select(u => new { u.Id, u.UserName }));
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersByRoles()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var projectManagers = await _userManager.GetUsersInRoleAsync("ProjectManager");
            var teamMembers = await _userManager.GetUsersInRoleAsync("TeamMember");

            return Ok(new
            {
                Admins = admins.Select(u => new { u.Id, u.UserName }),
                ProjectManagers = projectManagers.Select(u => new { u.Id, u.UserName }),
                TeamMembers = teamMembers.Select(u => new { u.Id, u.UserName })
            });
        }
    }
}
