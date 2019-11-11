using Microsoft.AspNetCore.Builder;

namespace KickSport.Web.SeedData.Extensions
{
    public static class SeedCategoriesExtension
    {
        public static IApplicationBuilder UseSeedCategories(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedCategories>();
        }
    }
}
