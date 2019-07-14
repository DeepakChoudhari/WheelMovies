using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WheelMovies.Business;
using WheelMovies.Business.DTO;
using WheelMovies.Business.Interfaces;

namespace WheelMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IMovieService movieService;
        private ILogger<MoviesController> logger;

        public MoviesController(IMovieService movieService, ILogger<MoviesController> logger)
        {
            this.movieService = movieService;
            this.logger = logger;
        }

        // GET api/movies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MoviesResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MoviesResponse>>> Get()
        {
            var movies = await movieService.GetTop5MoviesAsync();
            if (!movies.Any())
                return NotFound("No movies found.");

            return Ok(movies);
        }

        // GET api/movies/user/5
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MoviesResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MoviesResponse>>> Get(int? userId)
        {
            if (!userId.HasValue)
                return BadRequest("User id not specified in the request.");

            var movies = await movieService.GetTop5MoviesForUserAsync(userId.Value);
            if (!movies.Any())
                return NotFound("No movies found");

            return Ok(movies);
        }

        // POST api/movies
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MoviesResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MoviesResponse>>> Post([FromBody] GetMoviesByCriteriaRequest request)
        {
            var movies = await movieService.GetMoviesByCriteriaAsync(request);
            if (movies.Status == ResponseStatus.Invalid)
                return BadRequest("Invalid criteria");

            if (!movies.Movies.Any())
                return NotFound("No movies found");

            if (movies.Status == ResponseStatus.Fail)
                return new StatusCodeResult(500);

            return Ok(movies.Movies);
        }

        // PUT api/movies/rating/2
        [HttpPut("rating/{movieId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int movieId, [FromBody] AddUpdateUserRatingRequest request)
        {
            var status = await movieService.AddOrUpdateUserRatingForMovieAsync(movieId, request);
            if (status == ResponseStatus.NotFound)
                return NotFound("Movie or User not found");

            if (status == ResponseStatus.Invalid)
                return BadRequest("Invalid rating. Must be b/w 1 and 5");

            if (status == ResponseStatus.Fail)
                return new StatusCodeResult(500);

            return Ok();
        }
    }
}
