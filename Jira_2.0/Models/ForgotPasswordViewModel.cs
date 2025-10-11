using System.ComponentModel.DataAnnotations;

namespace Jira_2._0.Models
{
    // Models/ForgotPasswordViewModel.cs
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Registered Email")]
        public string Email { get; set; }
    }
}
