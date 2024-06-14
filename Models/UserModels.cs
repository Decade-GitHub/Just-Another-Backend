namespace AccessControlAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class UserRequestModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}