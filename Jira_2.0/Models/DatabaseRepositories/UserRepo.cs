using Jira_2._0.Data;
using Jira_2._0.Models.CustomisedUserModel;
using Microsoft.AspNetCore.Identity;
using Jira_2._0.Models.Context;
using Microsoft.AspNetCore.Identity.UI.Services;
using Jira_2._0.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace Jira_2._0.Models.DatabaseRepositories
{

    public class UserRepo:IUserRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserRepo(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                        ApplicationDbContext context,
                        INotificationService notificationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _notificationService = notificationService;
        }


        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }


        public async Task ApproveClaimAsync(string userId, string claim)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return;

            user.IsClaimVerified = true;
            await _userManager.AddClaimAsync(user, new Claim("ClaimType", claim));
            _context.Update(user);
            await _context.SaveChangesAsync();

            await _notificationService.SendClaimApprovalNotificationAsync(user, claim);
        }

        public async Task RejectClaimAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return;

            user.IsRejected = true;
            _context.Update(user);
            await _context.SaveChangesAsync();

            await _notificationService.SendClaimRejectionNotificationAsync(user);
        }



        //[Authorize]
        //public async Task ApproveClaimAsync(string userId, string claim)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        throw new Exception("User not found");

        //    // Update user properties
        //    user.RequestedClaim = claim;
        //    user.IsClaimVerified = true;
        //    user.IsRejected = false;

        //    // Update the user in the database
        //    await _userManager.UpdateAsync(user);

        //    // Remove existing claims if they exist
        //    var existingClaims = await _userManager.GetClaimsAsync(user);
        //    var requestedClaim = existingClaims.FirstOrDefault(c => c.Type == "RequestedClaim");
        //    var isVerifiedClaim = existingClaims.FirstOrDefault(c => c.Type == "IsClaimVerified");
        //    var isRejectedClaim = existingClaims.FirstOrDefault(c => c.Type == "IsRejected");
        //    var userTypeClaim = existingClaims.FirstOrDefault(c => c.Type == "UserType");

        //    if (requestedClaim != null)
        //        await _userManager.RemoveClaimAsync(user, requestedClaim);
        //    if (isVerifiedClaim != null)
        //        await _userManager.RemoveClaimAsync(user, isVerifiedClaim);
        //    if (isRejectedClaim != null)
        //        await _userManager.RemoveClaimAsync(user, isRejectedClaim);
        //    if (userTypeClaim != null)
        //        await _userManager.RemoveClaimAsync(user, userTypeClaim);

        //    // Add updated claims
        //    await _userManager.AddClaimAsync(user, new Claim("RequestedClaim", claim));
        //    await _userManager.AddClaimAsync(user, new Claim("IsClaimVerified", "1"));
        //    await _userManager.AddClaimAsync(user, new Claim("IsRejected", "0"));
        //    await _userManager.AddClaimAsync(user, new Claim("UserType", claim));
        //    await _context.SaveChangesAsync();
        //    await _signInManager.RefreshSignInAsync(user);
        //    await _notificationService.SendClaimRejectionNotificationAsync(user);

        //}
        //[Authorize]
        //public async Task RejectClaimAsync(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        throw new Exception("User not found");

        //    // Update user properties
        //    user.IsRejected = true;

        //    // Update the user in the database
        //    await _userManager.UpdateAsync(user);

        //    // Remove existing claims if they exist
        //    var existingClaims = await _userManager.GetClaimsAsync(user);
        //    var isRejectedClaim = existingClaims.FirstOrDefault(c => c.Type == "IsRejected");

        //    if (isRejectedClaim != null)
        //        await _userManager.RemoveClaimAsync(user, isRejectedClaim);

        //    // Add updated claim
        //    await _userManager.AddClaimAsync(user, new Claim("IsRejected", "1"));
        //    await _context.SaveChangesAsync();
        //    await _signInManager.RefreshSignInAsync(user);
        //    await _notificationService.SendClaimRejectionNotificationAsync(user);
        //}

        public async Task<List<ApplicationUser>> GetPendingClaimRequestsAsync()
        {
            return await _context.Users
                .Where(u => !u.IsClaimVerified && !u.IsRejected && u.RequestedClaim != null)
                .ToListAsync();
        }

        public List<ApplicationUser> GetAllActiveUsers()
        {
            return _context.Users
                           .Where(u => u.IsRejected == false)
                           .ToList();
        }

    }
}
