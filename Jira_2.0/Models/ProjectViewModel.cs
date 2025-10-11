using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jira_2._0.Models
{
    public class ProjectViewModel
    {
        public ProjectModelWrapper ProjectDetails;

        public List<SelectListItem> ProjectManagers { get; set; }
        public List<SelectListItem> TeamMembers { get; set; }

        public string SelectedProjectManagerId { get; set; }
        public List<string> SelectedTeamMemberIds { get; set; }
    }
}
