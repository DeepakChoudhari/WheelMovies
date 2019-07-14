using System.Collections.Generic;

namespace WheelMovies.Business.DTO
{
    public class GetMoviesByCriteriaResponse
    {
        public ResponseStatus Status { get; set; }

        public IEnumerable<MoviesResponse> Movies { get; set; }
    }
}
