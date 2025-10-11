using Jira_2._0.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
namespace Jira_2._0.Interfaces
{
    public interface IProjectRepo
    {
        public int Insert(ProjectModelWrapper wrapper);
        public bool UpdateProject(ProjectModelWrapper wrapper);
        public bool DeleteProject(int id);
        public List<ProjectModelWrapper> GetAllProjects();
        public ProjectModelWrapper GetProjectById(int id);
        public ProjectDetailsViewModel GetProjectDetails(int projectId);
    }
}
