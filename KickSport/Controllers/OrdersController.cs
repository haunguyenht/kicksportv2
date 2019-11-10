using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KickSport.Data.Models;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Orders;
using KickSport.Web.Models.Common;
using KickSport.Web.Models.Orders.InputModels;
using KickSport.Web.Models.Orders.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KickSport.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OrdersController : ApiController
    {
        private readonly IProductsService _productsService;
        private readonly IOrdersService _ordersService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public OrdersController(
            IProductsService productsService,
            IOrdersService ordersService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _productsService = productsService;
            _ordersService = ordersService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SuccessViewModel<OrderViewModel>>> Submit([FromBody] OrderInputModel model)
        {
            foreach (var product in model.OrderProducts)
            {
                if (!await _productsService.Exists(product.Id))
                {
                    return BadRequest(new BadRequestViewModel
                    {
                        Message = $"Product with id {product.Id} not found."
                    });
                }
            }

            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var orderProducts = model.OrderProducts
                    .Select(op => _mapper.Map<OrderProductDto>(op))
                    .ToList();

                var orderDto = await _ordersService.CreateOrderAsync(user.Id, orderProducts);

                return new SuccessViewModel<OrderViewModel>
                {
                    Data = _mapper.Map<OrderViewModel>(orderDto),
                    Message = "Your order was processed successfully."
                };
            }
            catch (Exception)
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "Something went wrong."
                });
            }
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> My()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                return _ordersService
                    .GetUserOrders(user.Id)
                    .Select(orderDto => _mapper.Map<OrderViewModel>(orderDto))
                    .ToList();
            }
            catch (Exception)
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "Something went wrong."
                });
            }
        }
    }
}