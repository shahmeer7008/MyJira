using Jira_2._0.Data;
using Jira_2._0.Models.DatabaseRepositories;
using Jira_2._0.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Jira_2._0.Models.Context;
using Jira_2._0.Models;
using Jira_2._0.Hubs;
using Jira_2._0;
using Jira_2._0.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Jira_2._0.Models.CustomisedUserModel;
using Microsoft.AspNetCore.SignalR;
using System.Configuration;
var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseSqlServer(
        connectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));
builder.Services.AddDbContext<JiraDBContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS-only
    options.Cookie.HttpOnly = true; // Prevent JS access
    options.Cookie.SameSite = SameSiteMode.None; // Allow cross-site usage, required for external logins or cross-origin apps
});

builder.Services.AddSignalR().AddHubOptions<NotificationHub>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.EnableDetailedErrors = true;
   
});

// ------------------ CLAIM-BASED POLICIES ------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireClaim("UserType", "Admin"));

    options.AddPolicy("RequireProjectManager", policy =>
        policy.RequireClaim("UserType", "ProjectManager"));

    options.AddPolicy("RequireTeamMember", policy =>
        policy.RequireClaim("UserType", "TeamMember"));

    
        options.AddPolicy("id", policy =>
            policy.RequireClaim("id")); // No need to pass a value if you're only checking for existence
    

});
// ----------------------------------------------------------

builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
builder.Services.AddScoped<IIssueRepo, IssueRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();
builder.Services.AddScoped<SignInManager<ApplicationUser>, CustomSignInManager>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Seed roles and users (this should now include adding claims too)
using (var scope = app.Services.CreateScope())
{
    await ClaimInitializer.Initialize(scope.ServiceProvider);
}

app.MapHub<NotificationHub>("/notificationHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");
app.MapRazorPages();

app.Run();
