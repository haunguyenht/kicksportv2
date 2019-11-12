using KickSport.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Web.SeedData
{
    public class SeedIngredients
    {
        private readonly RequestDelegate _next;

        public SeedIngredients(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider)
        {
            var ingredientsService = provider.GetService<IIngredientsService>();
            if (!await ingredientsService.Any())
            {
                await ingredientsService.CreateRangeAsync(new string[]
                {
                    "leather",
                    "plastic",
                    "wood",
                    "gold",
                    "platinum",
                    "ham",
                    "silver",
                    "cloth",
                    "cotton",
                    "leather",
                    "silk",
                    "rubber",
                    "suede",
                    "boost",
                    "sock-like",
                    "primeknit"
                });
            }

            await _next(context);
        }
    }
}
