namespace WheelMovies.Repository.Models
{
    public partial class MovieRating
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public short Rating { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }
    }
}
