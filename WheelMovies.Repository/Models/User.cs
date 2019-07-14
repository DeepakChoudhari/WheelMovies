using System.Collections.Generic;

namespace WheelMovies.Repository.Models
{
    public partial class User
    {
        public User()
        {
            MovieRating = new HashSet<MovieRating>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual ICollection<MovieRating> MovieRating { get; set; }
    }
}
