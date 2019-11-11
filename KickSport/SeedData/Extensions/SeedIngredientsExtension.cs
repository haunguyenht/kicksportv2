using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Web.SeedData.Extensions
{
    public static class SeedIngredientsExtension
    {
        public static IApplicationBuilder UseSeedIngredients(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedIngredients>();
        }
    }
}
