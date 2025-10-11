using System;
using System.ComponentModel.DataAnnotations;

namespace Jira_2._0.Models
{
    public class IssueModel
    {
        [Required]

        public int ProjectID { get; set; }

        [Required]

        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Type { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Assigned { get; set; }

        
        public string CreatedOn { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public string LastUpdated { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


    }
}
