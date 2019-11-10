using System;

namespace KickSport.Web.Models.Reviews.ViewModels
{
    public class ReviewViewModel
    {
        public string Id { get; set; }

        public string ReviewText { get; set; }

        public string CreatorUsername { get; set; }

        public DateTime LastModified { get; set; }
    }
}
