using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Web.Controllers;
using KickSport.Web.Models.Common;
using KickSport.Web.Models.Ingredients.ViewModels;
using KickSport.Web.Areas.Models.Ingredients.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KickSport.Web.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
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

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SuccessViewModel<IngredientViewModel>>> Post([FromBody] IngredientInputModel model)
        {
            if (User.IsInRole("Administrator"))
            {
                try
                {
                    await _ingredientsService.CreateAsync(model.Name);

                    var createdIngredientDto = await _ingredientsService.FindByName(model.Name);

                    return new SuccessViewModel<IngredientViewModel>
                    {
                        Data = _mapper.Map<IngredientViewModel>(createdIngredientDto),
                        Message = "Ingredient added successfully."
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

            return Unauthorized();
        }
    }
}