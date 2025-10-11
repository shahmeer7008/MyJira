namespace Jira_2._0.Models
{
    public class UsersModel
    {
        int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UsersModel(string Name,string Email,string Password)
        {

            this.Name = Name;
            this.Email = Email; 
            this.Password = Password;
        }
    }
}
