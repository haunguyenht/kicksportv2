using KickSport.Data;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KickSport.Services.DataServices.Tests
{
    public class ProductsIngredientsServiceTests
    {
        private readonly IGenericRepository<ProductsIngredients> _productsIngredientsRepository;
        private readonly ProductsIngredientsService _productsIngredientsService;

        public ProductsIngredientsServiceTests()
        {
            var options = new DbContextOptionsBuilder<KickSportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new KickSportDbContext(options);

            _productsIngredientsRepository = new GenericRepository<ProductsIngredients>(dbContext);
            _productsIngredientsService = new ProductsIngredientsService(_productsIngredientsRepository);
        }

        [Fact]
        public async Task DeleteProductIngredientsAsyncShouldDeleteProductIngredientsSuccessfully()
        {
            var productIngredients = new List<ProductsIngredients>()
            {
                new ProductsIngredients
                {
                    IngredientId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"),
                    ProductId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28b")
                },
                new ProductsIngredients
                {
                    IngredientId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28a"),
                    ProductId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28b")
                },
                new ProductsIngredients
                {
                    IngredientId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28e"),
                    ProductId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28f")
                }
            };

            await _productsIngredientsRepository.AddRangeAsync(productIngredients);
            await _productsIngredientsRepository.SaveChangesAsync();

            await _productsIngredientsService.DeleteProductIngredientsAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28b"));

            var productsIngredients = await _productsIngredientsRepository.GetAllAsync();
            Assert.Equal(1, await _productsIngredientsRepository.CountAsync());

            var productIngredient = productsIngredients.First();
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28e"), productIngredient.IngredientId);
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28f"), productIngredient.ProductId);
        }
    }
}
