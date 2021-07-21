using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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

        public RatDbContext(string connectionString)
            : this(new DbContextOptionsBuilder<RatDbContext>().UseSqlServer(connectionString).Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
                {
                    entity.ToTable("Projects");
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).ValueGeneratedOnAdd();

                    entity.Property(e => e.Name).HasMaxLength(248).IsRequired();

                    entity.HasOne(e => e.Type);
                    entity.HasMany(e => e.Users).WithMany(u => u.Projects);
                });

            modelBuilder.Entity<User>(builer =>
            {
                builer.ToTable("Users");

                builer.HasKey(x => x.Id);
                builer.Property(x => x.Id).ValueGeneratedOnAdd();

                builer.Property(x => x.UserId).HasMaxLength(128).IsRequired();
            });

            modelBuilder.Entity<ProjectType>(builder =>
            {
                builder.ToTable("ProjectTypes");

                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.Property(x => x.Name).HasMaxLength(64).IsRequired();

                builder.HasIndex(x => x.Name).IsUnique();

                builder.HasData(new List<ProjectType>
                {
                    new ProjectType {Id = 1, Name = "other"},
                    new ProjectType {Id = 2, Name = "js"},
                    new ProjectType {Id = 3, Name = "csharp"}
                });
            });
        }
    }

    internal class RatDesignTimeDbContextFactory : IDesignTimeDbContextFactory<RatDbContext>
    {
        public RatDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RatDbContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=RatDb;User ID=sa;Password=Password1!;Connect Timeout=30");

            return new RatDbContext(optionsBuilder.Options);
        }
    }
}
