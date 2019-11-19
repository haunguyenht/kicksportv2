using AutoMapper;
using KickSport.Data;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Helpers;
using KickSport.Services.DataServices.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KickSport.Services.DataServices.Tests
{
    public class ProductsServiceTests
    {
        private readonly IGenericRepository<Product> _productsRepository;
        private readonly ProductsService _productsService;
        private readonly KickSportDbContext _dbContext;

        public ProductsServiceTests()
        {
            var options = new DbContextOptionsBuilder<KickSportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new KickSportDbContext(options);

            var mapperProfile = new MappingConfiguration();
            var conf = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(conf);

            _productsRepository = new GenericRepository<Product>(_dbContext);
            _productsService = new ProductsService(_productsRepository, mapper);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateProductSuccessfully()
        {
            var diablo = new ProductDto
            {
                Name = "Diablo",
                Description = "Test",
                Price = 8.90m,
                Weight = 500,
                Image = "https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg"
            };

            await _productsService.CreateAsync(diablo);

            Assert.Equal(1, await _productsRepository.CountAsync());

            var product = await _productsRepository.FirstAsync();
            Assert.Equal("Diablo", product.Name);
            Assert.Equal("Test", product.Description);
            Assert.Equal(8.90m, product.Price);
            Assert.Equal(500, product.Weight);
            Assert.Equal("https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg", product.Image);
        }

        [Fact]
        public async Task CreateRangeAsyncShouldCreateProductsSuccessfully()
        {
            var products = new List<ProductDto>();

            var pollo = new ProductDto
            {
                Name = "Pollo",
                Description = "Test",
                Price = 10.90m,
                Weight = 550,
                Image = "http://www.ilforno.bg/45-large_default/polo.jpg"
            };
            products.Add(pollo);

            var diablo = new ProductDto
            {
                Name = "Diablo",
                Description = "Test",
                Price = 8.90m,
                Weight = 500,
                Image = "https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg"
            };
            products.Add(diablo);

            await _productsService.CreateRangeAsync(products);

            Assert.Equal(2, await _productsRepository.CountAsync());

            var firstProduct = await _productsRepository.FirstAsync();
            Assert.Equal("Pollo", firstProduct.Name);
            Assert.Equal("Test", firstProduct.Description);
            Assert.Equal(10.90m, firstProduct.Price);
            Assert.Equal(550, firstProduct.Weight);
            Assert.Equal("http://www.ilforno.bg/45-large_default/polo.jpg", firstProduct.Image);

            var secondProduct = await _productsRepository.LastAsync();
            Assert.Equal("Diablo", secondProduct.Name);
            Assert.Equal("Test", secondProduct.Description);
            Assert.Equal(8.90m, secondProduct.Price);
            Assert.Equal(500, secondProduct.Weight);
            Assert.Equal("https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg", secondProduct.Image);
        }

        [Fact]
        public async Task AllShouldReturnEmptyCollection()
        {
            var allProducts = await _productsService.All();
            Assert.Empty(allProducts);
        }

        [Fact]
        public async Task AllShouldReturnCorrectValues()
        {
            var products = new List<Product>();

            var pollo = new Product
            {
                Name = "Pollo",
                Description = "Test",
                Price = 10.90m,
                Weight = 550,
                Image = "http://www.ilforno.bg/45-large_default/polo.jpg",
                Category = new Category
                {
                    Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"),
                    Name = "Adidas"
                },
                Ingredients = new List<ProductsIngredients>
                {
                    new ProductsIngredients
                    {
                        IngredientId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"),
                        Ingredient = new Ingredient
                        {
                            Name = "boost"
                        }
                    }
                },
                Reviews = new List<Review>(),
                Likes = new List<UsersLikes>()
            };

            var diablo = new Product
            {
                Name = "Diablo",
                Description = "Test",
                Price = 8.90m,
                Weight = 500,
                Image = "https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg",
                Category = new Category
                {
                    Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28e"),
                    Name = "Nike"
                },
                Ingredients = new List<ProductsIngredients>
                {
                    new ProductsIngredients
                    {
                        IngredientId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28f"),
                        Ingredient = new Ingredient
                        {
                            Name = "cloth"
                        }
                    }
                },
                Reviews = new List<Review>(),
                Likes = new List<UsersLikes>()
            };

            products.Add(pollo);
            products.Add(diablo);

            await _productsRepository.AddRangeAsync(products);
            await _productsRepository.SaveChangesAsync();

            var allProducts = await _productsService.All();

            Assert.Equal(2, allProducts.Count);

            var firstProduct = allProducts.First();
            Assert.Equal("Pollo", firstProduct.Name);
            Assert.Equal("Test", firstProduct.Description);
            Assert.Equal(10.90m, firstProduct.Price);
            Assert.Equal(550, firstProduct.Weight);
            Assert.Equal("http://www.ilforno.bg/45-large_default/polo.jpg", firstProduct.Image);
            Assert.Equal("Adidas", firstProduct.Category.Name);

            var secondProduct = allProducts.Last();
            Assert.Equal("Diablo", secondProduct.Name);
            Assert.Equal("Test", secondProduct.Description);
            Assert.Equal(8.90m, secondProduct.Price);
            Assert.Equal(500, secondProduct.Weight);
            Assert.Equal("https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg", secondProduct.Image);
            Assert.Equal("Nike", secondProduct.Category.Name);
        }

        [Fact]
        public async Task AnyShouldReturnTrue()
        {
            var product = new ProductDto
            {
                Name = "Pollo",
                CategoryId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"),
                Description = "Test",
                Price = 10.90m,
                Weight = 550,
                Image = "http://www.ilforno.bg/45-large_default/polo.jpg"
            };

            await _productsService.CreateAsync(product);

            Assert.True(_productsService.Any());
        }

        [Fact]
        public void AnyShouldReturnFalse()
        {
            Assert.False(_productsService.Any());
        }

        [Fact]
        public async Task ExistsShouldReturnFalse()
        {
            Assert.False(await _productsService.Exists(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c")));
        }

        [Fact]
        public async Task ExistsShouldReturnTrue()
        {
            var diablo = new ProductDto
            {
                Name = "Diablo",
                Description = "Test",
                Price = 8.90m,
                Weight = 500,
                Image = "https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg"
            };

            await _productsService.CreateAsync(diablo);
            var productId = ( await _productsRepository.GetAllAsync()).First().Id;

            Assert.True(await _productsService.Exists(productId));
        }

        [Fact]
        public async Task EditAsyncShouldEditProductSuccessfully()
        {
            var product = new Product
            {
                Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"),
                Name = "Pollo",
                Description = "Test",
                Price = 10.90m,
                Weight = 550,
                Image = "http://www.ilforno.bg/45-large_default/polo.jpg",
                Category = new Category
                {
                    Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"),
                    Name = "Nike"
                },
                Ingredients = new List<ProductsIngredients>
                {
                    new ProductsIngredients
                    {
                        IngredientId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28e"),
                        Ingredient = new Ingredient
                        {
                            Name = "cloth"
                        }
                    }
                },
                Reviews = new List<Review>(),
                Likes = new List<UsersLikes>()
            };
            await _productsRepository.AddAsync(product);
            await _productsRepository.SaveChangesAsync();
            _dbContext.Entry(product).State = EntityState.Detached;

            var productDto = new ProductDto
            {
                Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"),
                Name = "Diablo2",
                Description = "test",
                Price = 9.90m,
                Weight = 400,
                Image = "http://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg"
            };

            await _productsService.EditAsync(productDto);

            var editedProduct = (await _productsRepository.GetAllAsync()).First();
            Assert.Equal("Diablo2", editedProduct.Name);
            Assert.Equal("test", editedProduct.Description);
            Assert.Equal(9.90m, editedProduct.Price);
            Assert.Equal(400, editedProduct.Weight);
            Assert.Equal("http://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg", editedProduct.Image);
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteProductSuccessfully()
        {
            var productDto = new ProductDto
            {
                Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"),
                Name = "Diablo",
                Description = "Test",
                Price = 8.90m,
                Weight = 500,
                Image = "https://images.pizza33.ua/products/product/yQfkJqZweoLn9omo68oz5BnaGzaIE0UJ.jpg"
            };

            await _productsService.CreateAsync(productDto);
            Assert.Equal(1,await _productsRepository.CountAsync());

            await _productsService.DeleteAsync(productDto.Id);
            Assert.Equal(0, await _productsRepository.CountAsync());
        }
    }
}
