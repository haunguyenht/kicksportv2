﻿using KickSport.Services.DataServices.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IOrdersService
    {
        Task ApproveOrderAsync(Guid orderId);

        Task<OrderDto> CreateOrderAsync(string userId, IEnumerable<OrderProductDto> orderProducts);

        Task<bool> Exists(Guid orderId);

        Task<List<OrderDto>> GetApprovedOrders();

        Task<List<OrderDto>> GetPendingOrders();

        Task<List<OrderDto>> GetUserOrders(string userId);

        Task DeleteProductOrdersAsync(Guid productId);
    }
}
