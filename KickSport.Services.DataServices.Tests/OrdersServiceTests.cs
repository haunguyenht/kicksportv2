using AutoMapper;
using KickSport.Data;
using KickSport.Data.Models;
using KickSport.Data.Models.Enums;
using KickSport.Data.Repository;
using KickSport.Helpers;
using KickSport.Services.DataServices.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KickSport.Services.DataServices.Tests
{
    public class OrdersServiceTests
    {
        private readonly IGenericRepository<Order> _ordersRepository;
        private readonly OrdersService _ordersService;

        public OrdersServiceTests()
        {
            var options = new DbContextOptionsBuilder<KickSportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new KickSportDbContext(options);

            var mapperProfile = new MappingConfiguration();
            var conf = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(conf);

            _ordersRepository = new GenericRepository<Order>(dbContext);
            var orderProductRepository = new GenericRepository<OrderProduct>(dbContext);
            _ordersService = new OrdersService(_ordersRepository, orderProductRepository, mapper);
        }

        [Fact]
        public async Task CreateOrderAsyncShouldCreateOrderSuccessfully()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            var createdOrderDto = await _ordersService.CreateOrderAsync("userID", orderProducts);

            Assert.Equal("userID", createdOrderDto.CreatorId);
            Assert.Equal(OrderStatus.Pending.ToString(), createdOrderDto.Status);
            Assert.Equal(2, createdOrderDto.OrderProducts.Count());

            var firstOrderProduct = createdOrderDto.OrderProducts.First();
            Assert.Equal(9.90m, firstOrderProduct.Price);
            Assert.Equal(1, firstOrderProduct.Quantity);

            var secondOrderProduct = createdOrderDto.OrderProducts.Last();
            Assert.Equal(10.90m, secondOrderProduct.Price);
            Assert.Equal(2, secondOrderProduct.Quantity);
        }

        [Fact]
        public async Task ExistsShouldReturnTrue()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);
            var orderId = (await _ordersRepository.FirstAsync()).Id;

            Assert.True(await _ordersService.Exists(orderId));
        }

        [Fact]
        public async Task ExistsShouldReturnFalse()
        {
            var orderId = Guid.NewGuid();
            Assert.False(await _ordersService.Exists(orderId));
        }

        [Fact]
        public async Task ApproveOrderAsyncShouldApproveOrderSuccessfully()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);
            var orderId = (await _ordersRepository.FirstAsync()).Id;

            await _ordersService.ApproveOrderAsync(orderId);

            Assert.Equal(OrderStatus.Approved, (await _ordersRepository.FirstAsync()).Status);
        }

        [Fact]
        public async Task DeleteProductOrdersAsyncShouldDeleteProductOrdersSuccessfully()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);

            await _ordersService.DeleteProductOrdersAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"));
            var order = await _ordersRepository.FirstAsync();

            Assert.Equal(1, order.Products.Count);
        }

        [Fact]
        public async Task GetUserOrdersShouldReturnUserOrdersCorrectly()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);

            var userOrders = await _ordersService.GetUserOrders("userID");

            Assert.Single(userOrders);
        }

        [Fact]
        public async Task GetUserOrdersShouldReturnEmptyCollection()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);

            var userOrders = await _ordersService.GetUserOrders("user");

            Assert.Empty(userOrders);
        }

        [Fact]
        public async Task GetPendingOrdersShouldReturnPendingOrdersCorrectly()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);
            await _ordersService.CreateOrderAsync("user", orderProducts);

            var pendingOrders = await _ordersService.GetPendingOrders();

            Assert.Equal(2, pendingOrders.Count);
        }

        [Fact]
        public async Task GetPendingOrdersShouldReturnEmptyCollection()
        {
            var pendingOrders = await _ordersService.GetPendingOrders();

            Assert.Empty(pendingOrders);
        }

        [Fact]
        public async Task GetApprovedOrdersShouldReturnApprovedOrdersCorrectly()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);
            var firstOrderId = (await _ordersRepository.FirstAsync()).Id;

            await _ordersService.CreateOrderAsync("user", orderProducts);
            var secondOrderId = (await _ordersRepository.LastAsync()).Id;

            await _ordersService.ApproveOrderAsync(firstOrderId);
            await _ordersService.ApproveOrderAsync(secondOrderId);

            var approvedOrders = await _ordersService.GetApprovedOrders();

            Assert.Equal(2, approvedOrders.Count);
        }

        [Fact]
        public async Task GetApprovedOrdersShouldReturnEmptyCollection()
        {
            var orderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Diablo",
                    Price = 9.90m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pollo",
                    Price = 10.90m,
                    Quantity = 2
                }
            };

            await _ordersService.CreateOrderAsync("userID", orderProducts);
            await _ordersService.CreateOrderAsync("user", orderProducts);

            var approvedOrders = await _ordersService.GetApprovedOrders();

            Assert.Empty(approvedOrders);
        }
    }
}
