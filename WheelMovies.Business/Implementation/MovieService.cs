using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WheelMovies.Business.DTO;
using WheelMovies.Business.Interfaces;
using WheelMovies.Repository;
using WheelMovies.Repository.Models;

namespace WheelMovies.Business.Implementation
{
    public class MovieService : IMovieService
    {
        private IMoviesRepository moviesRepository;
        private ILogger<MovieService> logger;

        public MovieService(IMoviesRepository moviesRepository, ILogger<MovieService> logger)
        {
            this.moviesRepository = moviesRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<MoviesResponse>> GetMoviesByCriteriaAsync(GetMoviesByCriteriaRequest request)
        {
            try
            {
                var movies = await this.moviesRepository.Movies
                    .Include(m => m.MovieGenre)
                    .ThenInclude(navigationPropertyPath: mg => mg.Genre)
                    .Include(m => m.MovieRating)
                    .Where(m => m.Title.Equals(request.Title, StringComparison.InvariantCultureIgnoreCase)
                             || m.YearOfRelease == request.YearOfRelease
                             || m.RunningTime == request.RunningTime)
                    .Select(m =>
                    new MoviesResponse
                    {
                        Id = m.Id,
                        Title = m.Title,
                        YearOfRelease = m.YearOfRelease,
                        RunningTime = m.RunningTime,
                        Genres = string.Join(", ", m.MovieGenre.Select(mg => mg.Genre.Name).ToList()),
                        AverageRating = Average(m.MovieRating)
                    }).ToListAsync();

                return movies;
            }
            catch (DbException dbException)
            {
                logger.LogError(dbException, $"Error occurred while retrieving movies for Title:{request.Title} RunningTime:{request.RunningTime} YearOfRelease:{request.YearOfRelease}");
                return Enumerable.Empty<MoviesResponse>();
            }
        }

        public async Task<IEnumerable<MoviesResponse>> GetTop5MoviesAsync()
        {
            try
            {
                //TODO: Investigate if this code can be optimized further as single query.
                var movies = await this.moviesRepository.Movies
                    .Include(m => m.MovieGenre)
                    .ThenInclude(navigationPropertyPath: mg => mg.Genre)
                    .Include(m => m.MovieRating)
                    .GroupBy(m => m.Id).Select(g => new { Movie = g.FirstOrDefault(), Avg = Average(g.FirstOrDefault().MovieRating) })
                    .OrderByDescending(m => m.Avg)
                    .ThenBy(m => m.Movie.Title)
                    .Take(5).ToListAsync();

                var moviesResponse = new List<MoviesResponse>();
                foreach (var m in movies)
                {
                    moviesResponse.Add(
                    new MoviesResponse
                    {
                        Id = m.Movie.Id,
                        Title = m.Movie.Title,
                        YearOfRelease = m.Movie.YearOfRelease,
                        RunningTime = m.Movie.RunningTime,
                        Genres = string.Join(", ", m.Movie.MovieGenre.Select(mg => mg.Genre.Name).ToList()),
                        AverageRating = Average(m.Movie.MovieRating)
                    });
                }
                return moviesResponse;
            }
            catch (DbException dbException)
            {
                logger.LogError(dbException, $"Error occurred while retrieving top 5 movies");
                return Enumerable.Empty<MoviesResponse>();
            }   
        }

        public async Task<IEnumerable<MoviesResponse>> GetTop5MmoviesForUserAsync(int userId)
        {
            try
            {
                var movies = await (from m in this.moviesRepository.Movies.AsNoTracking()
                .Include(m => m.MovieGenre)
                .ThenInclude(navigationPropertyPath: mg => mg.Genre)
                                    from r in this.moviesRepository.MovieRatings.AsNoTracking()
                                    where m.Id == r.MovieId && r.UserId == userId
                                    select new MoviesResponse
                                    {
                                        Id = m.Id,
                                        Title = m.Title,
                                        YearOfRelease = m.YearOfRelease,
                                        RunningTime = m.RunningTime,
                                        Genres = string.Join(", ", m.MovieGenre.Select(mg => mg.Genre.Name).ToList()),
                                        AverageRating = r.Rating
                                    })
                            .OrderByDescending(m => m.AverageRating)
                            .ThenBy(m => m.Title)
                            .Take(5)
                            .ToListAsync();

                return movies;
            }
            catch(DbException dbException)
            {
                logger.LogError(dbException, $"Error occurred while retrieving top 5 movies for user id: {userId}");
                return Enumerable.Empty<MoviesResponse>();
            }
        }

        public async Task<AddUpdateStatus> AddOrUpdateUserRatingForMovieAsync(int movieId,
            AddUpdateUserRatingRequest addUpdateUserRatingRequest)
        {
            try
            {
                var (isValid, addUpdateStatus) = await ValidateAddUpdateMovieRatingRequest(movieId, addUpdateUserRatingRequest);
                if (!isValid)
                    return addUpdateStatus;

                var movieRating = await this.moviesRepository.MovieRatings.Where(mr =>
                mr.MovieId == movieId &&
                mr.UserId == addUpdateUserRatingRequest.UserId).FirstOrDefaultAsync();

                if (movieRating == null)
                    await this.moviesRepository.MovieRatings.AddAsync(new MovieRating
                    {
                        MovieId = movieId,
                        UserId = addUpdateUserRatingRequest.UserId,
                        Rating = addUpdateUserRatingRequest.Rating
                    });
                else
                {
                    movieRating.Rating = addUpdateUserRatingRequest.Rating;
                    this.moviesRepository.MovieRatings.Update(movieRating);
                }

                return await this.moviesRepository.SaveChangesAsync() > 0 ? AddUpdateStatus.Success : AddUpdateStatus.Fail;
            }
            catch (DbException dbException)
            {
                logger.LogError(dbException, $"Error while adding/updating rating for the movie");
                return AddUpdateStatus.Fail;
            }
        }

        private async Task<(bool isValid, AddUpdateStatus addUpdateStatus)> ValidateAddUpdateMovieRatingRequest
            (int movieId, AddUpdateUserRatingRequest request)
        {
            var movieExists = await moviesRepository.Movies.AnyAsync(mr => mr.Id.Equals(movieId));
            if (!movieExists)
            {
                return (false, AddUpdateStatus.MovieOrUserNotFound);
            }

            var userExists = await moviesRepository.Users.AnyAsync(u => u.Id.Equals(request.UserId));
            if (!userExists)
            {
                return (false, AddUpdateStatus.MovieOrUserNotFound);
            }

            if (!(request.Rating >= 1 && request.Rating <= 5))
            {
                return (false, AddUpdateStatus.InvalidRating);
            }

            // addUpdateStatus = default;
            return (true, default(AddUpdateStatus));
        }

        private static double Average(ICollection<MovieRating> movieRating)
        {
            var avgVal = movieRating.Average(mr => mr.Rating);

            /*
             * Round the number to the closest 0.5
               3.249 should be displayed as 3.0;
               3.25 should be displayed as 3.5;
               3.6 should be displayed as 3.5;
               3.75 should be displayed as 4.0. 
             */
            return Math.Round(avgVal * 2, MidpointRounding.AwayFromZero) / 2;
        }
    }
}
