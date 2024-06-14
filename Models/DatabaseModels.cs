using Microsoft.EntityFrameworkCore;

namespace AccessControlAPI.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CEP> CEPs { get; set; }
    }
}
