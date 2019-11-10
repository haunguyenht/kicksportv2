using System.ComponentModel.DataAnnotations;
using static KickSport.Data.Common.Constant.Constants;

namespace KickSport.Web.Models.Reviews.InputModels
{
    public class CreateReviewInputModel
    {
        [Required]
        [MinLength(VALIDATION.ReviewMinimumLength, ErrorMessage = VALIDATION.ReviewErrorMessage)]
        public string Review { get; set; }
    }
}
