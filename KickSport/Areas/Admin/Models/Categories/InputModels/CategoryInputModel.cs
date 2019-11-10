using System.ComponentModel.DataAnnotations;
using static KickSport.Data.Common.Constant.Constants;

namespace KickSport.Web.Areas.Models.Categories.InputModels
{
    public class CategoryInputModel
    {
        [Required]
        [StringLength(VALIDATION.NameMaximumLength, MinimumLength = VALIDATION.NameMinimumLength, ErrorMessage = VALIDATION.NameErrorMessage)]
        public string Name { get; set; }
    }
}
