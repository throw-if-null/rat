using Microsoft.EntityFrameworkCore;
using Rat.Data.Views;
using Rat.DataAccess.ProjectTypes;

namespace Rat.DataAccess
{
    public class RatDbContext : DbContext
    {
        public DbSet<ProjectView> Projects { get; set; }
        public DbSet<UserView> Users { get; set; }

        public DbSet<ProjectType> ProjectTypes { get; set; }
    }
}
