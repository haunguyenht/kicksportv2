using System.Collections.Generic;

namespace KickSport.Web.Models.Products.ViewModels
{
    public class ProductViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Weight { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public string Category { get; set; }

        public IEnumerable<string> Ingredients { get; set; }

        public IEnumerable<string> Likes { get; set; }
    }
}
