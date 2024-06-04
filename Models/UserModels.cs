using Microsoft.EntityFrameworkCore;

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
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}