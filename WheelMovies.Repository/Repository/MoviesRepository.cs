using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WheelMovies.Repository.Models;

namespace WheelMovies.Repository
{
    public class MoviesRepository : IMoviesRepository
    {
        private WheelMoviesContext dbContext;

        public MoviesRepository(WheelMoviesContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbSet<Movie> Movies => this.dbContext.Movie;

        public DbSet<MovieRating> MovieRatings => this.dbContext.MovieRating;

        public DbSet<User> Users => this.dbContext.User;

        public async Task<int> SaveChangesAsync()
        {
            return await this.dbContext.SaveChangesAsync();
        }
    }
}
