using System;
using System.ComponentModel.DataAnnotations;

namespace Jira_2._0.Models
{
    public class ProjectModel
    {
        

        [Required(ErrorMessage = "Project name is required")]
        public string ProjectName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Client { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Leader { get; set; }

        [Required]
        public string Team { get; set; }

         public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Remove the Data property and serialization methods
    }
}