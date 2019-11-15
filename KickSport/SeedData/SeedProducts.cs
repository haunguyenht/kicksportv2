using KickSport.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using KickSport.Services.DataServices.Models.Products;
using KickSport.Services.DataServices.Models.Ingredients;

namespace KickSport.Web.SeedData
{
    public class SeedProducts
    {
        private readonly RequestDelegate _next;

        public SeedProducts(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider)
        {
            var productsService = provider.GetService<IProductsService>();

            if (!productsService.Any())
            {
                var ingredientsService = provider.GetService<IIngredientsService>();
                var categoriesService = provider.GetService<ICategoriesService>();
                var products = new List<ProductDto>();

                var nikeIngredients = new string[] { "leather", "cloth", "rubber" };
                var nike = new ProductDto
                {
                    Name = "Nike Airforce 1",
                    Description = "Designed by Bruce Kilgore and introduced in 1982, the Air Force 1 was the first ever basketball shoe to feature Nike Air technology, revolutionizing the game and sneaker culture forever.",
                    Price = 5.90m,
                    Weight = 350,
                    Image = "https://c.static-nike.com/a/images/t_PDP_1280_v1/f_auto/up7tdgif09cvhfiuxtta/air-force-1-07-womens-shoe-KyTwDPGG.jpg",
                    CategoryId = (await categoriesService.FindByName("Nike")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => nikeIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(nike);

                var adidasIngredients = new string[] { "primeknit", "sock-like", "boost", "rubber" };
                var adidas = new ProductDto
                {
                    Name = "Adidas Human Race",
                    Description = "The Adidas Pharrell Williams Human Race NMD is crafted with a breathable Primeknit upper, sock-like construction, energy-returning Boost technology, and a rubber outsole.",
                    Price = 9.90m,
                    Weight = 500,
                    Image = "https://www.festivalwalkmall.com/image/cache/catalog/NMD/AC7359/2-1300x1300.jpg",
                    CategoryId = (await categoriesService.FindByName("Adidas")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => adidasIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(adidas);

                var pumaIngredients = new string[] { "leather", "cloth", "rubber", "suede" };
                var puma = new ProductDto
                {
                    Name = "Puma Classic",
                    Description = "The PUMA Suede is definitely the most well-known and popular of all PUMA shoes and rightly deserves its place in every Hall of Fame.",
                    Price = 11.90m,
                    Weight = 500,
                    Image = "https://www.dressinn.com/f/128/1283869/puma-suede-classic.jpg",
                    CategoryId = (await categoriesService.FindByName("Puma")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => pumaIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(puma);

                var newbalanceIngredients = new string[] { "leather", "cloth", "rubber", "suede" };
                var newbalance = new ProductDto
                {
                    Name = "New Balance 997S Bodega No Bad Days",
                    Description = "(NB), best known as simply New Balance, is an American multinational corporation based in the Boston, Massachusetts area. The company was founded in 1906 as the New Balance Arch Support Company and is one of the world's major sports footwear and apparel manufacturers.",
                    Price = 10.90m,
                    Weight = 550,
                    Image = "http://www.thedopeuniversity.com/wp-content/uploads/2019/08/bodega-new-balance-1.jpg",
                    CategoryId = (await categoriesService.FindByName("New Balance")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => newbalanceIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(newbalance);

                var jordanIngredients = new string[] { "leather", "cloth", "rubber" };
                var jordan = new ProductDto
                {
                    Name = "Air Jordan 1 Bred",
                    Description = "Air Jordan is a brand of basketball shoes, athletic, casual, and style clothing produced by Nike.",
                    Price = 8.90m,
                    Weight = 500,
                    Image = "https://cdn.thesolesupplier.co.uk/2018/02/Jordan-1-Bred-Toe-555088-610-1.png",
                    CategoryId = (await categoriesService.FindByName("Air Jordan")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => jordanIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(jordan);

                var asicsIngredients = new string[] { "leather", "cloth", "rubber" };
                var asics = new ProductDto
                {
                    Name = "ASICS Gel-Kayano Trainer",
                    Description = "Asics (アシックス Ashikkusu) is a Japanese multinational corporation which produces footwear and sports equipment designed for a wide range of sports,",
                    Price = 13.90m,
                    Weight = 400,
                    Image = "http://kicksdeals.com/wp-content/uploads/2014/07/asics-gel-kayano-white-black-purple-1.jpg",
                    CategoryId = (await categoriesService.FindByName("Asics")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => asicsIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(asics);

                var converseIngredients = new string[] { "leather", "cloth", "rubber" };
                var converse = new ProductDto
                {
                    Name = "Converse 1970s",
                    Description = "Chuck Taylor All-Stars or Converse All Stars is a model of casual shoe manufactured by Converse that was initially developed as a basketball shoe in the early.",
                    Price = 9.80m,
                    Weight = 500,
                    Image = "https://images-na.ssl-images-amazon.com/images/I/71N5CqY9stL._UL1500_.jpg",
                    CategoryId = (await categoriesService.FindByName("Converse")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => converseIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(converse);

                var vansIngredients = new string[] { "leather", "cloth", "rubber", "suede" };
                var vans = new ProductDto
                {
                    Name = "Vans old skool vault",
                    Description = "Vans is an American manufacturer of skateboarding shoes and related apparel, based in Santa Ana, California and owned by VF Corporation.",
                    Price = 16.70m,
                    Weight = 420,
                    Image = "https://cdn.shopify.com/s/files/1/1853/3317/products/Vans-Vault-OG-Old-Skool-LX-Black-1-1024x1024_1024x1024_8481f3a4-4a41-481b-931b-3b17e268e68e_2048x.jpg?v=1543224437",
                    CategoryId = (await categoriesService.FindByName("Vans")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => vansIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(vans);

                var underArmourIngredients = new string[] { "leather", "cloth", "rubber", "suede", "plastic" };
                var underArmour = new ProductDto
                {
                    Name = "Under Armour Curry 7",
                    Description = "Under Armour, Inc. is an American company that manufactures footwear, sports, and casual apparel.",
                    Price = 9.90m,
                    Weight = 420,
                    Image = "https://images.solecollector.com/images/fl_lossy,q_auto/actzsiekudsqwvxworns/under-armour-curry-7-undrtd-lateral",
                    CategoryId = (await categoriesService.FindByName("Under Armour")).Id,
                    Ingredients = (await ingredientsService
                        .All())
                        .Where(i => underArmourIngredients.Contains(i.Name))
                        .Select(i => new IngredientDto { Id = i.Id, Name = i.Name })
                        .ToList()
                };
                products.Add(underArmour);

                await productsService.CreateRangeAsync(products);
            }

            await _next(context);
        }
    }
}
