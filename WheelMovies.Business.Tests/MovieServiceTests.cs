using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WheelMovies.Repository;
using WheelMovies.Business.Implementation;
using Microsoft.Extensions.Logging;
using WheelMovies.Business.DTO;
using System.Collections.Generic;
using WheelMovies.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace WheelMovies.Business.Tests
{
    public class MovieServiceTests
    {
        #region Tests for Get Movies By Criteria

        [Fact]
        public async Task GetMoviesByCriteriaAsync_ForInvalidRequest_ReturnsInvalidResponseState()
        {
            // 1. Arrange
            var moviesRepositoryMock = new Mock<IMoviesRepository>();
            var loggerMock = new Mock<ILogger<MovieService>>();
            var request = new GetMoviesByCriteriaRequest
            {
                Title = null
            };

            // 2. Act
            var movieService = new MovieService(moviesRepositoryMock.Object, loggerMock.Object);
            var response = await movieService.GetMoviesByCriteriaAsync(request);

            // 3. Assert
            Assert.Equal(ResponseStatus.Invalid, response.Status);
            Assert.True(!response.Movies.Any());
        }

        [Fact]
        public async Task GetMoviesByCriteriaAsync_ForValidRequest_ReturnsMoviesList()
        {
            // 1. Arrange
            var options = new DbContextOptionsBuilder<WheelMoviesContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var moviesContext = new WheelMoviesContext(options);
            moviesContext.Movie.Add(new Movie
            {
                Id = 1,
                Title = "Movie1",
                YearOfRelease = 2000,
                RunningTime = 120,
                MovieGenre = new List<MovieGenre>
                        {
                            new MovieGenre { Id = 1, GenreId = 1, MovieId = 1 }
                        }
            });
            moviesContext.Movie.Add(new Movie
            {
                Id = 2,
                Title = "Movie2",
                YearOfRelease = 2001,
                RunningTime = 120,
                MovieGenre = new List<MovieGenre>
                        {
                            new MovieGenre { Id = 2, GenreId = 1, MovieId = 2 }
                        }
            });
            await moviesContext.SaveChangesAsync();

            var moviesRepository = new MoviesRepository(moviesContext);
            var loggerMock = new Mock<ILogger<MovieService>>();
            var request = new GetMoviesByCriteriaRequest
            {
                YearOfRelease = 2000
            };

            // 2. Act
            var movieService = new MovieService(moviesRepository, loggerMock.Object);
            var response = await movieService.GetMoviesByCriteriaAsync(request);

            // 3. Assert
            Assert.NotEmpty(response.Movies);
            Assert.True(response.Movies.Count() == 1);
        }

        #endregion

        #region Tests for GetTop5MmoviesForUserAsync

        [Fact]
        public async Task GetTop5MoviesForUserAsync_ReturnsTop5MoviesFor_A_User()
        {
            // 1. Arrange
            var options = new DbContextOptionsBuilder<WheelMoviesContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var moviesContext = new WheelMoviesContext(options);
            moviesContext.User.Add(new User
            {
                Id = 1,
                Name = "User1"
            });
            moviesContext.User.Add(new User
            {
                Id = 2,
                Name = "User2"
            });
            moviesContext.Movie.Add(new Movie
            {
                Id = 1,
                Title = "Movie1",
                YearOfRelease = 2000,
                RunningTime = 120,
                MovieGenre = new List<MovieGenre>
                        {
                            new MovieGenre { Id = 1, GenreId = 1, MovieId = 1 }
                        }
            });
            moviesContext.Movie.Add(new Movie
            {
                Id = 2,
                Title = "Movie2",
                YearOfRelease = 2001,
                RunningTime = 120,
                MovieGenre = new List<MovieGenre>
                        {
                            new MovieGenre { Id = 2, GenreId = 1, MovieId = 2 }
                        }
            });
            moviesContext.MovieRating.Add(new MovieRating
            {
                Id = 1,
                MovieId = 1,
                UserId = 1,
                Rating = 3
            });
            moviesContext.MovieRating.Add(new MovieRating
            {
                Id = 2,
                MovieId = 2,
                UserId = 1,
                Rating = 4
            });

            await moviesContext.SaveChangesAsync();

            var moviesRepository = new MoviesRepository(moviesContext);
            var loggerMock = new Mock<ILogger<MovieService>>();
            var request = new GetMoviesByCriteriaRequest
            {
                YearOfRelease = 2000
            };

            // 2. Act
            var movieService = new MovieService(moviesRepository, loggerMock.Object);
            var response = await movieService.GetTop5MoviesForUserAsync(1);

            // 3. Assert
            Assert.NotEmpty(response);
            Assert.True(response.Count() == 2);
        }

        #endregion

    }
}
