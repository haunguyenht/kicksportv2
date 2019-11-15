using System;
using System.Collections.Generic;
namespace KickSport.Services.DataServices.Models.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public string CreatorId { get; set; }

        public string CreatorEmail { get; set; }

        public DateTime CreationDate { get; set; }

        public string Status { get; set; }

        public IEnumerable<OrderProductDto> OrderProducts { get; set; }
    }
}
