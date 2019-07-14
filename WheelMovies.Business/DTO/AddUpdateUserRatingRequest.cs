namespace WheelMovies.Business.DTO
{
    public class AddUpdateUserRatingRequest
    {
        public int UserId { get; set; }

        public short Rating { get; set; }
    }
}
