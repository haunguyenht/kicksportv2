namespace KickSport.Services.DataServices.Models.Reviews
{
    using System;

    public class ReviewDto
    {
        public Guid Id { get; set; }

        public string ReviewText { get; set; }

        public string CreatorUsername { get; set; }

        public DateTime LastModified { get; set; }
    }
}
