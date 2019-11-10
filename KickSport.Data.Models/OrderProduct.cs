using KickSport.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class OrderProduct : BaseModel<int>
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
