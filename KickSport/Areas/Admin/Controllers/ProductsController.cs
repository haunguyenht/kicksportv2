﻿using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Ingredients;
using KickSport.Services.DataServices.Models.Products;
using KickSport.Web.Controllers;
using KickSport.Web.Hubs;
using KickSport.Web.Hubs.Contracts;
using KickSport.Web.Models.Common;
using KickSport.Web.Models.Products.ViewModels;
using KickSport.Web.Areas.Models.Products.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Web.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    public class ProductsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IIngredientsService _ingredientsService;
        private readonly IReviewsService _reviewsService;
        private readonly IProductsIngredientsService _productsIngredientsService;
        private readonly IUsersLikesService _usersLikesService;
        private readonly IOrdersService _ordersService;
        private readonly IHubContext<ProductsHub, IProductsHubClient> _productsHubContext;

        public ProductsController(
            IMapper mapper,
            IProductsService productsService,
            ICategoriesService categoriesService,
            IIngredientsService ingredientsService,
            IReviewsService reviewsService,
            IProductsIngredientsService productsIngredientsService,
            IUsersLikesService usersLikesService,
            IOrdersService ordersService,
            IHubContext<ProductsHub, IProductsHubClient> productHubContext)
        {
            _mapper = mapper;
            _productsService = productsService;
            _categoriesService = categoriesService;
            _ingredientsService = ingredientsService;
            _reviewsService = reviewsService;
            _productsIngredientsService = productsIngredientsService;
            _usersLikesService = usersLikesService;
            _ordersService = ordersService;
            _productsHubContext = productHubContext;
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Post([FromBody] ProductInputModel model)
        {
            if (User.IsInRole("Administrator"))
            {
                if (_productsService.All().Any(p => p.Name == model.Name))
                {
                    return BadRequest(new BadRequestViewModel
                    {
                        Message = "Product with the given name already exists."
                    });
                }

                var productCategory = _categoriesService.FindByName(model.Category);
                if (productCategory != null)
                {
                    var ingredients = new List<IngredientDto>();
                    foreach (var ingredientName in model.Ingredients)
                    {
                        var ingredient = await _ingredientsService.FindByName(ingredientName);
                        if (ingredient != null)
                        {
                            ingredients.Add(ingredient);
                        }
                        else
                        {
                            return BadRequest(new BadRequestViewModel
                            {
                                Message = $"{ingredientName} ingredient not found."
                            });
                        }
                    }

                    var productDto = new ProductDto
                    {
                        Name = model.Name,
                        CategoryId = productCategory.Id,
                        Description = model.Description,
                        Image = model.Image,
                        Weight = model.Weight,
                        Price = model.Price,
                        Ingredients = ingredients.Select(i => new IngredientDto
                        {
                            Id = i.Id,
                            Name = i.Name
                        }).ToList()
                    };

                    try
                    {
                        await _productsService.CreateAsync(productDto);
                        var createdProductDto = _productsService
                            .All()
                            .First(p => p.Name == productDto.Name);

                        var createdProductViewModel = _mapper.Map<ProductViewModel>(createdProductDto);

                        await _productsHubContext.Clients.All.BroadcastProduct(createdProductViewModel);

                        return Ok(new
                        {
                            Message = "Product added successfully."
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

                return BadRequest(new BadRequestViewModel
                {
                    Message = "Category not found."
                });
            }

            return Unauthorized();
        }

        [HttpPut("{productId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SuccessViewModel<ProductViewModel>>> Put([FromRoute] string productId, [FromBody] ProductInputModel model)
        {
            if (User.IsInRole("Administrator"))
            {
                if (!await _productsService.Exists(productId))
                {
                    return BadRequest(new BadRequestViewModel
                    {
                        Message = "Product with the given id does not exist."
                    });
                }

                var productCategory = _categoriesService.FindByName(model.Category);
                if (productCategory != null)
                {
                    var productDto = _productsService
                        .All()
                        .First(p => p.Id == productId);

                    if (productDto.Name != model.Name && _productsService.All().Any(p => p.Name == model.Name))
                    {
                        return BadRequest(new BadRequestViewModel
                        {
                            Message = "Product with the given name already exists."
                        });
                    }

                    var ingredients = new List<IngredientDto>();
                    foreach (var ingredientName in model.Ingredients)
                    {
                        var ingredient = await _ingredientsService.FindByName(ingredientName);
                        if (ingredient != null)
                        {
                            ingredients.Add(ingredient);
                        }
                        else
                        {
                            return BadRequest(new BadRequestViewModel
                            {
                                Message = $"{ingredientName} ingredient not found."
                            });
                        }
                    }

                    await _productsIngredientsService
                        .DeleteProductIngredientsAsync(productId);

                    productDto.Name = model.Name;
                    productDto.CategoryId = productCategory.Id;
                    productDto.Description = model.Description;
                    productDto.Image = model.Image;
                    productDto.Weight = model.Weight;
                    productDto.Price = model.Price;
                    productDto.Ingredients = ingredients.Select(i => new IngredientDto
                    {
                        Id = i.Id,
                        Name = i.Name
                    }).ToList();

                    try
                    {
                        await _productsService.EditAsync(productDto);
                        var editedProductDto = _productsService
                            .All()
                            .First(p => p.Name == productDto.Name);

                        return new SuccessViewModel<ProductViewModel>
                        {
                            Data = _mapper.Map<ProductViewModel>(editedProductDto),
                            Message = "Product edited successfully."
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

                return BadRequest(new BadRequestViewModel
                {
                    Message = "Category not found."
                });
            }

            return Unauthorized();
        }

        [HttpDelete("{productId}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(string productId)
        {
            if (User.IsInRole("Administrator"))
            {
                if (!await _productsService.Exists(productId))
                {
                    return BadRequest(new BadRequestViewModel
                    {
                        Message = "Product with the given id does not exist."
                    });
                }

                try
                {
                    await _usersLikesService.DeleteProductLikesAsync(productId);
                    await _productsIngredientsService.DeleteProductIngredientsAsync(productId);
                    await _reviewsService.DeleteProductReviewsAsync(productId);
                    await _ordersService.DeleteProductOrdersAsync(productId);
                    await _productsService.DeleteAsync(productId);

                    return Ok(new
                    {
                        Message = "Product deleted successfully."
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

            return Unauthorized();
        }
    }
}