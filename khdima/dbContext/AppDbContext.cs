
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
    }
}
