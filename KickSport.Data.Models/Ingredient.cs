using KickSport.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class Ingredient : BaseModel<Guid>
    {
        public string Name { get; set; }

        public ICollection<ProductsIngredients> Products { get; set; }
    }
}
