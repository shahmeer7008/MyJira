using Jira_2._0.Models.CustomisedUserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jira_2._0.Models
{
    public class ResolvedIssues
    {
       
        [Required]
        public int ProjectID { get; set; }

        [Required]
        
        public int IssueID { get; set; }

        [Required]
        public string ResolvedUserId { get; set; } // FK to AspNetUsers (ApplicationUser)

        // Navigation properties
       

        [ForeignKey("ProjectID")]
        public ProjectModelWrapper Project { get; set; }

        [ForeignKey("IssueID")]
        public IssueModelWrapper Issue { get; set; }
    }
}
