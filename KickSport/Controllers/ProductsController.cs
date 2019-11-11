using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KickSport.Data.Models;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Products;
using KickSport.Web.Models.Common;
using KickSport.Web.Models.Products.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KickSport.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProductsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsersLikesService _usersLikesService;

        public ProductsController(
            IMapper mapper,
            IProductsService productsService,
            UserManager<ApplicationUser> userManager,
            IUsersLikesService usersLikesService)
        {
            _mapper = mapper;
            _productsService = productsService;
            _userManager = userManager;
            _usersLikesService = usersLikesService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> All()
        {
            var result = await _productsService.All();
            var productView = _mapper.Map<List<ProductDto>, List<ProductViewModel>>(result);

            return Ok(productView);
        }

        [HttpPost("{productId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Like(string productId)
        {
            if (!await _productsService.Exists(productId))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "Product not found."
                });
            }

            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = await _productsService.All();
                var product = result.First(p => p.Id == productId);

                if (product.Likes.Any(u => u.Id == user.Id))
                {
                    return BadRequest(new BadRequestViewModel
                    {
                        Message = "You have already liked this product."
                    });
                }

                await _usersLikesService.CreateUserLikeAsync(productId, user.Id);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "Something went wrong."
                });
            }
        }

        [HttpPost("{productId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Unlike(string productId)
        {
            if (!await _productsService.Exists(productId))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "Product not found."
                });
            }

            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                await _usersLikesService.DeleteUserLikeAsync(productId, user.Id);

                return Ok();
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