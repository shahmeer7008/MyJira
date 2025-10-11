using Jira_2._0.Models;

namespace Jira_2._0.Interfaces
{
    public interface IIssueRepo
    {
        public int Insert(IssueModelWrapper wrapper);
        public bool UpdateIssue(IssueModelWrapper wrapper);
        public bool DeleteIssue(int id);
        public List<IssueModelWrapper> GetAllIssues();
        public IssueModelWrapper GetIssueById(int id);
        public List<IssueModelWrapper> SearchIssues(string projectId, string priority, string status);

        public Task<List<IssueModelWrapper>> GetAssignedIssuesForUserAsync(string userId);

        public Task<bool> UpdateIssueStatusAndPriorityAsync(int IssueID, string Status, string priority, string Title, int ProjectID, string Id);

        public  Task<List<ResolvedIssueViewModel>> GetResolvedIssuesByUserAsync(string userId);

        public List<IssueModelWrapper> GetAllIssuesForManager(string name);
    }
}
