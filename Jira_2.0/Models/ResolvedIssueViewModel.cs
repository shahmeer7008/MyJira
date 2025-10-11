using System.Text;

namespace Jira_2._0.Models
{
    public class ResolvedIssueViewModel
    {
        public int IssueID { get; set; }
        public string Title { get; set; }
        public string ResolvedBy { get; set; }
        public string Priority { get; set; }
        public String DateResolved { get; set; }
    }

}
