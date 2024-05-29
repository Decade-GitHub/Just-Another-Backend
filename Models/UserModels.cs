using Microsoft.EntityFrameworkCore;

namespace AcessControlAPI.Models
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
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}