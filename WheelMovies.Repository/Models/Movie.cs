using System.Collections.Generic;

namespace WheelMovies.Repository.Models
{
    public partial class Movie
    {
        public Movie()
        {
            MovieGenre = new HashSet<MovieGenre>();
            MovieRating = new HashSet<MovieRating>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public short YearOfRelease { get; set; }
        public short RunningTime { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual ICollection<MovieGenre> MovieGenre { get; set; }
        public virtual ICollection<MovieRating> MovieRating { get; set; }
    }
}
