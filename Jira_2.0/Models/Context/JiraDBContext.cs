using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Text.Json.Serialization;
using System.Text.Json;
using Jira_2._0.Models;
namespace Jira_2._0.Models.Context
{
    public class JiraDBContext:DbContext
    {
        public JiraDBContext(DbContextOptions<JiraDBContext> options)
           : base(options)
        {
        }
        public DbSet<ProjectModelWrapper> ProjectsData { get; set; }
        public DbSet<IssueModelWrapper> Issues { get; set; }
        public DbSet<AssignedIssues> AssignedIssues { get; set; }

        public DbSet<ResolvedIssues> ResolvedIssues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var projectConverter = new ValueConverter<ProjectModel, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<ProjectModel>(v, (JsonSerializerOptions)null)
            );

            var issueConverter = new ValueConverter<IssueModel, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<IssueModel>(v, (JsonSerializerOptions)null)
            );
            modelBuilder.Entity<ProjectModelWrapper>()
                .HasKey(e => e.ProjectID);
            modelBuilder.Entity<ProjectModelWrapper>()
                .ToTable("ProjectsData")
                .Property(e => e.ProjectData)
                .HasConversion(projectConverter)
                .HasColumnName("Data");

            modelBuilder.Entity<ProjectModelWrapper>()
                .Property(e => e.ProjectID)
                .HasColumnName("ProjectID");

            modelBuilder.Entity<IssueModelWrapper>()
                .HasKey(e => e.IssueID);
            modelBuilder.Entity<IssueModelWrapper>()
                .ToTable("Issues")
                .Property(e => e.IssueData)
                .HasConversion(issueConverter)
                .HasColumnName("Data");

            modelBuilder.Entity<IssueModelWrapper>()

                .Property(e => e.IssueID)
                .HasColumnName("IssueID");


            modelBuilder.Entity<ResolvedIssues>()
         .HasKey(r => new { r.IssueID, r.ProjectID });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AssignedIssues>()
        .HasKey(r => new { r.IssueID, r.ProjectID });

            base.OnModelCreating(modelBuilder);





        }
}
}
