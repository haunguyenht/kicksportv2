using System.ComponentModel.DataAnnotations;
using static KickSport.Data.Common.Constant.Constants;

namespace KickSport.Web.Areas.Models.Ingredients.InputModels
{
    public class IngredientInputModel
    {
        [Required]
        [StringLength(VALIDATION.NameMaximumLength, MinimumLength = VALIDATION.NameMinimumLength, ErrorMessage = VALIDATION.NameErrorMessage)]
        public string Name { get; set; }
    }
}
