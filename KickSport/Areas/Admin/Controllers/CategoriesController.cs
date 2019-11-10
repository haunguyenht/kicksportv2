using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Web.Controllers;
using KickSport.Web.Models.Categories.ViewModels;
using KickSport.Web.Models.Common;
using KickSport.Web.Areas.Models.Categories.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KickSport.Web.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    public class CategoriesController : ApiController
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICategoriesService categoriesService,
            IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SuccessViewModel<CategoryViewModel>>> Post([FromBody] CategoryInputModel model)
        {
            if (User.IsInRole("Administrator"))
            {
                try
                {
                    await _categoriesService.CreateAsync(model.Name);

                    var createdCategoryDto = _categoriesService.FindByName(model.Name);

                    return new SuccessViewModel<CategoryViewModel>
                    {
                        Data = _mapper.Map<CategoryViewModel>(createdCategoryDto),
                        Message = "Category added successfully."
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