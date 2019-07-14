using Microsoft.EntityFrameworkCore;
using WheelMovies.Repository.Models;

namespace WheelMovies.Repository
{
    public partial class WheelMoviesContext : DbContext
    {
        public WheelMoviesContext()
        {
        }

        public WheelMoviesContext(DbContextOptions<WheelMoviesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieGenre> MovieGenre { get; set; }
        public virtual DbSet<MovieRating> MovieRating { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_MovieGenre_Genre");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK_MovieGenre_Movie");
            });

            modelBuilder.Entity<MovieRating>(entity =>
            {
                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieRating)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK_MovieRating_Movie");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MovieRating)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MovieRating_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });
        }
    }
}
