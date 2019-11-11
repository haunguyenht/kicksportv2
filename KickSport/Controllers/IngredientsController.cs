using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Ingredients;
using KickSport.Web.Models.Ingredients.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KickSport.Web.Controllers
{
    public class IngredientsController : ApiController
    {
        private readonly IIngredientsService _ingredientsService;
        private readonly IMapper _mapper;

        public IngredientsController(
            IIngredientsService ingredientsService,
            IMapper mapper)
        {
            _ingredientsService = ingredientsService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _ingredientsService.All();

            var ingredientView = _mapper.Map<List<IngredientDto>, List<IngredientViewModel>>(result);
            return Ok(ingredientView);
        }
    }
}