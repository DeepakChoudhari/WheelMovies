using System.Collections.Generic;
using System.Threading.Tasks;
using WheelMovies.Business.DTO;

namespace WheelMovies.Business.Interfaces
{
    public interface IMovieService
    {
        Task<ResponseStatus> AddOrUpdateUserRatingForMovieAsync(int movieId, 
            AddUpdateUserRatingRequest addUpdateUserRatingRequest);

        Task<GetMoviesByCriteriaResponse> GetMoviesByCriteriaAsync(GetMoviesByCriteriaRequest request);

        Task<IEnumerable<MoviesResponse>> GetTop5MoviesForUserAsync(int userId);

        Task<IEnumerable<MoviesResponse>> GetTop5MoviesAsync();
    }
}