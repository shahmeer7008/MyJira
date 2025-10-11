namespace Jira_2._0.Models
{
    // ViewModels/ProjectDetailsViewModel.cs

    public class ProjectDetailsViewModel
    {
        public ProjectModelWrapper Project { get; set; }
        public List<IssueModelWrapper> Issues { get; set; } = new List<IssueModelWrapper>();
    }

}
