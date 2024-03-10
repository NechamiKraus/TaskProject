namespace Tasks.Models
{
    public class User
    {
        public long UserId {get;set;}
        public string Username { get; set; }

        public string Password { get; set; }

        public bool TaskManager {get;set;}
    }
}
