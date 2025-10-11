using Jira_2._0.Models.CustomisedUserModel;
using System.ComponentModel.DataAnnotations;

namespace Jira_2._0.Models
{
    public class AssignedIssues
    {
        
        [Required]
        public int ProjectID { get; set; }           // FK to Project

        [Required]
        public int IssueID { get; set; }             // FK to Issue

        [Required]
        public string AssignedUserId { get; set; }   // FK to AspNetUsers (ApplicationUser)

        // Navigation properties
    
        public ProjectModelWrapper Project { get; set; }
        public IssueModelWrapper Issue { get; set; }
    }

}
