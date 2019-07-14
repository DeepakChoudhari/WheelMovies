using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WheelMovies.Repository.Models;

namespace WheelMovies.Repository
{
    public interface IMoviesRepository
    {
        DbSet<Movie> Movies { get; }

        DbSet<MovieRating> MovieRatings { get; }

        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync();
    }
}
