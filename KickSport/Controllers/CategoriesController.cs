using AutoMapper;
using KickSport.Services.DataServices.Contracts;
using KickSport.Web.Models.Categories.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public ActionResult<IEnumerable<CategoryViewModel>> Get()
        {
            var x = _categoriesService
                .All()
                .Select(c => _mapper.Map<CategoryViewModel>(c))
                .ToList();
            return x;
        }
    }
}