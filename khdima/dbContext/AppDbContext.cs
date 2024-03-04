
using khdima.Models;
using Microsoft.EntityFrameworkCore;

namespace khdima.dbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<User_has_role> User_has_role { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
