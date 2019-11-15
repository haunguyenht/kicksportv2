using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class ProductsIngredients
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public Guid IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
