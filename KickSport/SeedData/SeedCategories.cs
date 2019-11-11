using KickSport.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Web.SeedData
{
    public class SeedCategories
    {
        private readonly RequestDelegate _next;

        public SeedCategories(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider)
        {
            var categoriesService = provider.GetService<ICategoriesService>();
            if (!await categoriesService.Any())
            {
                await categoriesService.CreateRangeAsync(new string[]
                {
                    "Vegetarian",
                    "Traditional",
                    "Italian",
                    "Premium",
                    "American"
                });
            }

            await _next(context);
        }
    }
}
