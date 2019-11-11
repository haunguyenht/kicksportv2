using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KickSport.Data.Models;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Reviews;
using KickSport.Web.Models.Common;
using KickSport.Web.Models.Reviews.InputModels;
using KickSport.Web.Models.Reviews.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KickSport.Web.Controllers
{
    public class ReviewsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly IReviewsService _reviewsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(
            IMapper mapper,
            IProductsService productsService,
            IReviewsService reviewsService,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _productsService = productsService;
            _reviewsService = reviewsService;
            _userManager = userManager;
        }

        [HttpGet("{productId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ReviewViewModel>>> Get(string productId)
        {
            if (!await _productsService.Exists(productId))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "Product not found."
                });
            }

            var result = _reviewsService.GetProductReviews(productId);
            var reviewView = _mapper.Map<List<ReviewDto>,List<ReviewViewModel>>(result);
            return Ok(reviewView);
        }

        [HttpPost("{productId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SuccessViewModel<ReviewViewModel>>> Post([FromRoute] string productId, [FromBody] CreateReviewInputModel model)
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
                var creator = await _userManager.FindByNameAsync(User.Identity.Name);
                var review = await _reviewsService.CreateAsync(model.Review, creator.Id, productId);

                return new SuccessViewModel<ReviewViewModel>
                {
                    Message = "Review added successfully.",
                    Data = _mapper.Map<ReviewViewModel>(review)
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

        [HttpDelete("{reviewId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(string reviewId)
        {
            if (!await _reviewsService.Exists(reviewId))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = $"Review with id {reviewId} was not found."
                });
            }

            var reviewCreatorName = _reviewsService.FindReviewCreatorById(reviewId);
            if (!User.IsInRole("Administrator") && User.Identity.Name != reviewCreatorName)
            {
                return Unauthorized();
            }

            try
            {
                await _reviewsService.DeleteReviewAsync(reviewId);

                return Ok(new
                {
                    Message = "Review deleted successfully."
                });
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
