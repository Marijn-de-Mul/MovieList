using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ML.SAL.DTO;
using ML.SAL.Models;

namespace ML.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserDTO> Users { get; set; }
        public DbSet<MovieListDTO> MovieLists { get; set; }
        public DbSet<MovieDTO> Movies { get; set; }
        public DbSet<MovieListMoviesDTO> MovieListMovies { get; set; }
        public DbSet<MovieListSharedWithDTO> MovieListSharedWith { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieListMoviesDTO>()
                .HasKey(mlm => new { mlm.MovieListId, mlm.MovieId });

            modelBuilder.Entity<MovieListMoviesDTO>()
                .HasOne(mlm => mlm.MovieList)
                .WithMany(ml => ml.Movies)
                .HasForeignKey(mlm => mlm.MovieListId);

            modelBuilder.Entity<MovieListMoviesDTO>()
                .HasOne(mlm => mlm.Movie)
                .WithMany()
                .HasForeignKey(mlm => mlm.MovieId);

            modelBuilder.Entity<MovieListSharedWithDTO>()
                .HasKey(mlsw => new { mlsw.MovieListId, mlsw.UserId });

            modelBuilder.Entity<MovieListSharedWithDTO>()
                .HasOne(mlsw => mlsw.MovieList)
                .WithMany(ml => ml.SharedWith)
                .HasForeignKey(mlsw => mlsw.MovieListId);

            modelBuilder.Entity<MovieListSharedWithDTO>()
                .HasOne(mlsw => mlsw.User)
                .WithMany()
                .HasForeignKey(mlsw => mlsw.UserId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}