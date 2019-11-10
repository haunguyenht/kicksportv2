using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Web.Models.Ingredients.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public async Task<ActionResult<IEnumerable<IngredientViewModel>>> Get()
        {
            return _ingredientsService
                .All()
                .Select(i => _mapper.Map<IngredientViewModel>(i))
                .ToList();
        }
    }
}