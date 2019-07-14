namespace WheelMovies.Business.DTO
{
    public class GetMoviesByCriteriaRequest
    {
        public string Title { get; set; }

        public int? YearOfRelease { get; set; }

        public int? RunningTime { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(Title) && !YearOfRelease.HasValue && !RunningTime.HasValue)
                return false;

            return true;
        }
    }
}
