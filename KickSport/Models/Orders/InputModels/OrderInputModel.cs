using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KickSport.Web.Models.Orders.InputModels
{
    public class OrderInputModel
    {
        [Required]
        public IEnumerable<OrderProductInputModel> OrderProducts { get; set; }
    }
}
