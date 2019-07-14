using System.Collections.Generic;

namespace WheelMovies.Repository.Models
{
    public partial class Genre
    {
        public Genre()
        {
            MovieGenre = new HashSet<MovieGenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual ICollection<MovieGenre> MovieGenre { get; set; }
    }
}
