using KickSport.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class Product : BaseModel<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Weight { get; set; }

        public string Image { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<ProductsIngredients> Ingredients { get; set; }

        public ICollection<UsersLikes> Likes { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
