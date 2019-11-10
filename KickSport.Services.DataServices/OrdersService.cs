using AutoMapper;
using KickSport.Data.Models;
using KickSport.Data.Models.Enums;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Orders;
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

        public async Task ApproveOrderAsync(string orderId)
        {
            //var order = _ordersRepository
            //    .All()
            //    .First(o => o.Id == orderId);

            //order.Status = OrderStatus.Approved;
            //await _ordersRepository.SaveChangesAsync();
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
                .All()
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .First(o => o.Id == order.Id);

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public bool Exists(string orderId)
        {
            return _ordersRepository
                .All()
                .Any(o => o.Id == orderId);
        }

        public IEnumerable<OrderDto> GetApprovedOrders()
        {
            return _ordersRepository
                .All()
                .Include(o => o.Creator)
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .Where(o => o.Status == OrderStatus.Approved)
                .Select(o => _mapper.Map<OrderDto>(o));
        }

        public IEnumerable<OrderDto> GetPendingOrders()
        {
            return _ordersRepository
                .All()
                .Include(o => o.Creator)
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .Where(o => o.Status == OrderStatus.Pending)
                .Select(o => _mapper.Map<OrderDto>(o));
        }

        public IEnumerable<OrderDto> GetUserOrders(string userId)
        {
            return _ordersRepository
                .All()
                .Include(o => o.Creator)
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .Where(o => o.CreatorId == userId)
                .Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task DeleteProductOrdersAsync(string productId)
        {
            var orderProducts = _orderProductRepository
                .All()
                .Where(op => op.ProductId == productId)
                .ToList();

            _orderProductRepository.DeleteRange(orderProducts);

            await _orderProductRepository.SaveChangesAsync();
        }
    }
}

