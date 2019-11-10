using KickSport.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KickSport.Data
{
    public class KickSportDbContext : IdentityDbContext<ApplicationUser>
    {
        public KickSportDbContext(DbContextOptions<KickSportDbContext> options)
            : base(options) { }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
