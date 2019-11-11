using Microsoft.AspNetCore.Builder;

namespace KickSport.Web.SeedData.Extensions
{
    public static class SeedProductsExtension
    {
        public static IApplicationBuilder UseSeedProducts(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedProducts>();
        }
    }
}