using KickSport.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class Category : BaseModel<int>
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
