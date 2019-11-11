using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Categories;
using KickSport.Web.Models.Categories.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KickSport.Web.Controllers
{
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

        [HttpGet]
        [AllowAnonymous]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _categoriesService.All();
            var categoryView = _mapper.Map<List<CategoryDto>, List<CategoryViewModel>>(result);

            return Ok(categoryView);

        }
    }
}