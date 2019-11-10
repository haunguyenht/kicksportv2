﻿using KickSport.Services.DataServices.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IOrdersService
    {
        Task ApproveOrderAsync(string orderId);

        Task<OrderDto> CreateOrderAsync(string userId, IEnumerable<OrderProductDto> orderProducts);

        Task<bool> Exists(string orderId);

        IEnumerable<OrderDto> GetApprovedOrders();

        IEnumerable<OrderDto> GetPendingOrders();

        IEnumerable<OrderDto> GetUserOrders(string userId);

        Task DeleteProductOrdersAsync(string productId);
    }
}
