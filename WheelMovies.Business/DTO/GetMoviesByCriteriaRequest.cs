using System;
using System.Collections.Generic;
using System.Text;

namespace WheelMovies.Business.DTO
{
    public class GetMoviesByCriteriaRequest
    {
        public string Title { get; set; }

        public int YearOfRelease { get; set; }

        public int RunningTime { get; set; }
    }
}
