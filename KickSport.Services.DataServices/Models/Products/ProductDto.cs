using KickSport.Data.Models;
using KickSport.Services.DataServices.Models.Categories;
using KickSport.Services.DataServices.Models.Ingredients;
using System;
using System.Collections.Generic;
namespace KickSport.Services.DataServices.Models.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Weight { get; set; }

        public string Image { get; set; }

        public Guid CategoryId { get; set; }

        public CategoryDto Category { get; set; }

        public IEnumerable<IngredientDto> Ingredients { get; set; }

        public IEnumerable<ApplicationUser> Likes { get; set; }
    }
}
