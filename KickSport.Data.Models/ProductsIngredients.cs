﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class ProductsIngredients
    {
        public int Id { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }

        public int IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
