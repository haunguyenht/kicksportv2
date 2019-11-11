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
                    "olive oil",
                    "oregano",
                    "pepperoni salami",
                    "yellow cheese",
                    "tomato sauce",
                    "ham",
                    "mushrooms",
                    "smoked cheese",
                    "traditional bulgarian flat sausage called lukanka",
                    "chicken roll",
                    "corn",
                    "red peppers",
                    "hot peppers",
                    "chicken",
                    "avocado",
                    "olives",
                    "pineapple",
                    "white bulgarian cheese",
                    "blue cheese",
                    "philadelphia",
                    "tuna fish",
                    "white pepper",
                    "cherry tomatoes",
                    "basil chips",
                    "chorizo",
                    "proschuitto",
                    "eggs",
                    "bacon",
                    "red onion",
                    "mozzarella",
                    "parsley"
                });
            }

            await _next(context);
        }
    }
}
