using Microsoft.EntityFrameworkCore;
using Rat.Data.Entities;

namespace Rat.Data
{
    public class RatDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ProjectType> ProjectTypes { get; set; }

        public RatDbContext(DbContextOptions<RatDbContext> options) : base(options)
        {
        }
    }
}
