using KickSport.Data.Common;
using KickSport.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class Order : BaseModel<string>
    {
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<OrderProduct> Products { get; set; }
    }
}
