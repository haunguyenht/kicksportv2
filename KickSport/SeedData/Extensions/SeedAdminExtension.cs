using Microsoft.AspNetCore.Builder;

namespace KickSport.Web.SeedData.Extensions
{
    public static class SeedAdminExtension
    {
        public static IApplicationBuilder UseSeedAdmin(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedAdmin>();
        }
    }
}
