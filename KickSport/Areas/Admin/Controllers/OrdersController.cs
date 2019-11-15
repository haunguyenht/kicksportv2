using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Web.Controllers;
using KickSport.Web.Models.Common;
using KickSport.Web.Models.Orders.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Web.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]/[action]")]
    public class OrdersController : ApiController
    {
        private readonly IOrdersService _ordersService;
        private readonly IMapper _mapper;

        public OrdersController(
            IOrdersService ordersService,
            IMapper mapper)
        {
            _ordersService = ordersService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Pending()
        {
            if (User.IsInRole("Administrator"))
            {

                var result = await _ordersService.GetPendingOrders();
                var orderview = _mapper.Map<List<OrderViewModel>>(result.ToList()).ToList();

                return Ok(orderview);
                //return _ordersService
                //    .GetPendingOrders()
                //    .Select(orderDto => _mapper.Map<OrderViewModel>(orderDto))
                //    .ToList();
            }

            return Unauthorized();
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Approved()
        {
            if (User.IsInRole("Administrator"))
            {
                var result = await _ordersService.GetApprovedOrders();
                var orderview = _mapper.Map<List<OrderViewModel>>(result.ToList()).ToList();

                return Ok(orderview);

                //return _ordersService
                //    .GetApprovedOrders()
                //    .Select(orderDto => _mapper.Map<OrderViewModel>(orderDto))
                //    .ToList();
            }

            return Unauthorized();
        }

        [HttpPost("{orderId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Approve(Guid orderId)
        {
            if (User.IsInRole("Administrator"))
            {
                if (await _ordersService.Exists(orderId))
                {
                    await _ordersService.ApproveOrderAsync(orderId);

                    return Ok(new
                    {
                        Message = "Order approved successfully."
                    });
                }

                return BadRequest(new BadRequestViewModel
                {
                    Message = "Order not found."
                });
            }

            return Unauthorized();
        }
    }
}