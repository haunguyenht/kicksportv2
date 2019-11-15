using System;
using System.ComponentModel.DataAnnotations;
using static KickSport.Data.Common.Constant.Constants;

namespace KickSport.Web.Models.Orders.InputModels
{
    public class OrderProductInputModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(VALIDATION.MinimumPrice, Double.MaxValue, ErrorMessage = VALIDATION.PriceErrorMessage)]
        public decimal Price { get; set; }

        [Range(VALIDATION.MinimumQuantity, int.MaxValue, ErrorMessage = VALIDATION.QuantityErrorMessage)]
        public int Quantity { get; set; }
    }
}
