using AutoMapper;
using KickSport.Data.Models;
using KickSport.Data.Models.Enums;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices
{
    public class OrdersService : IOrdersService
    {
        private readonly IGenericRepository<Order> _ordersRepository;
        private readonly IGenericRepository<OrderProduct> _orderProductRepository;
        private readonly IMapper _mapper;

        public OrdersService(
            IGenericRepository<Order> ordersRepository,
            IGenericRepository<OrderProduct> orderProductRepository,
            IMapper mapper)
        {
            _ordersRepository = ordersRepository;
            _orderProductRepository = orderProductRepository;
            _mapper = mapper;
        }

        public async Task ApproveOrderAsync(Guid orderId)
        {
            var order = await _ordersRepository.FindOneAsync(o => o.Id == orderId);
            order.Status = OrderStatus.Approved;
            await _ordersRepository.SaveChangesAsync();
        }

        public async Task<OrderDto> CreateOrderAsync(string userId, IEnumerable<OrderProductDto> orderProducts)
        {
            var order = new Order
            {
                CreatorId = userId,
                CreationDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Products = orderProducts
                    .Select(op => _mapper.Map<OrderProduct>(op))
                    .ToList()
            };

            await _ordersRepository.AddAsync(order);
            await _ordersRepository.SaveChangesAsync();

            var createdOrder = _ordersRepository
                .DbSet
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .First(o => o.Id == order.Id);
            var orderDto = _mapper.Map<OrderDto>(createdOrder);
            return orderDto;
        }

        public async Task<bool> Exists(Guid orderId)
        {
            var existOrder = await _ordersRepository.FindOneAsync(o => o.Id == orderId);
            return existOrder != null;
        }

        public async Task<List<OrderDto>> GetApprovedOrders()
        {
            var getApproveOrders = await _ordersRepository
                .DbSet
                .Include(o => o.Creator)
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .Where(o => o.Status == OrderStatus.Approved)
                .ToListAsync();
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(getApproveOrders).ToList();
            return ordersDto;
        }

        public async Task<List<OrderDto>> GetPendingOrders()
        {
            var getPedingOrders = await _ordersRepository
                .DbSet
                .Include(o => o.Creator)
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .Where(o => o.Status == OrderStatus.Pending)
                .ToListAsync();
            var ordersDto = _mapper.Map<List<OrderDto>>(getPedingOrders).ToList();
            return ordersDto;
        }

        public async Task<List<OrderDto>> GetUserOrders(string userId)
        {
            var getUserOrders = await _ordersRepository
                .DbSet
                .Include(o => o.Creator)
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .Where(o => o.CreatorId == userId)
                .ToListAsync();
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(getUserOrders).ToList();
            return ordersDto;
        }

        public async Task DeleteProductOrdersAsync(Guid productId)
        {
            var orderProducts = _orderProductRepository
                .DbSet
                .Where(op => op.ProductId == productId)
                .ToList();

            await _orderProductRepository.DeleteRange(orderProducts);
            await _orderProductRepository.SaveChangesAsync();
        }
    }
}

