using System;

namespace KickSport.Services.DataServices.Models.Orders
{
    public class OrderProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
