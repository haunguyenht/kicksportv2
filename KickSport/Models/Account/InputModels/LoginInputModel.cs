using System.ComponentModel.DataAnnotations;

namespace KickSport.Web.Models.Account.InputModels
{
    public class LoginInputModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
