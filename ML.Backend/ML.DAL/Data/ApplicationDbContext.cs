using Microsoft.EntityFrameworkCore;
using ML.SAL.Models;

namespace ML.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<MovieList> MovieLists { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieList>()
                .HasMany(ml => ml.Movies)
                .WithOne()
                .HasForeignKey(m => m.Id);

            modelBuilder.Entity<MovieList>()
                .HasMany(ml => ml.SharedWith)
                .WithOne()
                .HasForeignKey(u => u.Id);
        }
    }
}