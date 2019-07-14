namespace WheelMovies.Business.DTO
{
    public class MoviesResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int YearOfRelease { get; set; }

        public int RunningTime { get; set; }

        public string Genres { get; set; }

        public double AverageRating { get; set; }
    }
}
