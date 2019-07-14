﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WheelMovies.Business.DTO;

namespace WheelMovies.Business.Interfaces
{
    public interface IMovieService
    {
        Task<AddUpdateStatus> AddOrUpdateUserRatingForMovieAsync(int movieId, 
            AddUpdateUserRatingRequest addUpdateUserRatingRequest);

        Task<IEnumerable<MoviesResponse>> GetMoviesByCriteriaAsync(GetMoviesByCriteriaRequest request);

        Task<IEnumerable<MoviesResponse>> GetTop5MmoviesForUserAsync(int userId);

        Task<IEnumerable<MoviesResponse>> GetTop5MoviesAsync();
    }
}