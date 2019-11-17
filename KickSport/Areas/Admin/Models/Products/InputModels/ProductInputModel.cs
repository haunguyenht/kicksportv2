using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static KickSport.Data.Common.Constant.Constants;

namespace KickSport.Web.Areas.Models.Products.InputModels
{
    public class ProductInputModel
    {
        [Required]
        [StringLength(VALIDATION.NameMaximumLength, MinimumLength = VALIDATION.NameMinimumLength, ErrorMessage = VALIDATION.NameErrorMessage)]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public List<string> Ingredients { get; set; }

        [Required]
        [StringLength(VALIDATION.DescriptionMaximumLength, MinimumLength = VALIDATION.DescriptionMinimumLength, ErrorMessage = VALIDATION.DescriptionErrorMessage)]
        public string Description { get; set; }


        public IFormFile File { get; set; }

        [Range(VALIDATION.MinimumWeight, VALIDATION.MaximumWeight, ErrorMessage = VALIDATION.WeightErrorMessage)]
        public int Weight { get; set; }

        [Range(VALIDATION.MinimumPrice, Double.MaxValue, ErrorMessage = VALIDATION.PriceErrorMessage)]
        public decimal Price { get; set; }
    }
}
