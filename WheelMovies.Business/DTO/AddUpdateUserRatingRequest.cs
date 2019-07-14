using System;
using System.Collections.Generic;
using System.Text;

namespace WheelMovies.Business.DTO
{
    public class AddUpdateUserRatingRequest
    {
        public int UserId { get; set; }

        public short Rating { get; set; }
    }
}
