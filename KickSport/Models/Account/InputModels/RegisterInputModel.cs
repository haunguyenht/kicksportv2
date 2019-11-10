using System.ComponentModel.DataAnnotations;
using static KickSport.Data.Common.Constant.Constants;

namespace KickSport.Web.Models.Account.InputModels
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress(ErrorMessage = VALIDATION.EmailErrorMessage)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(VALIDATION.UsernameRegex, ErrorMessage = VALIDATION.UsernameRegexErrorMessage)]
        [MinLength(VALIDATION.UsernameMinimumLength, ErrorMessage = VALIDATION.UsernameErrorMessage)]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [MinLength(VALIDATION.PasswordMinimumLength, ErrorMessage = VALIDATION.PasswordErrorMessage)]
        public string Password { get; set; }
    }
}
