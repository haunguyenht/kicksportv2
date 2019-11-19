using AutoMapper;
using KickSport.Data;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KickSport.Services.DataServices.Tests
{
    public class IngredientsServiceTests
    {
        private readonly IGenericRepository<Ingredient> _ingredientsRepository;
        private readonly IngredientsService _ingredientsService;

        public IngredientsServiceTests()
        {
            var options = new DbContextOptionsBuilder<KickSportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new KickSportDbContext(options);

            var mapperProfile = new MappingConfiguration();
            var conf = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(conf);

            _ingredientsRepository = new GenericRepository<Ingredient>(dbContext);
            _ingredientsService = new IngredientsService(_ingredientsRepository, mapper);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateIngredientSuccessfully()
        {
            await _ingredientsService.CreateAsync("boost");

            Assert.Equal(1, await _ingredientsRepository.CountAsync());
            Assert.Equal("boost", (await _ingredientsRepository.FirstAsync()).Name);
        }

        [Fact]
        public async Task CreateRangeAsyncShouldCreateIngredientsSuccessfully()
        {
            await _ingredientsService.CreateRangeAsync(new string[]
            {
                "boost", "primeknit"
            });

            Assert.Equal(2, await _ingredientsRepository.CountAsync());
            Assert.Equal("boost", (await _ingredientsRepository.FirstAsync()).Name);
            Assert.Equal("primeknit", (await _ingredientsRepository.LastAsync()).Name);
        }

        [Fact]
        public async Task FindByNameShouldReturnNull()
        {
            var ingredient = await _ingredientsService.FindByName("boost");
            Assert.Null(ingredient);
        }

        [Fact]
        public async Task FindByNameShouldReturnCorrectValues()
        {
            await _ingredientsService.CreateAsync("boost");

            var ingredient = await _ingredientsService.FindByName("boost");
            Assert.Equal("boost", ingredient.Name);
        }

        [Fact]
        public async Task AllShouldReturnEmptyCollection()
        {
            var allIngredients = await _ingredientsService.All();
            Assert.Empty(allIngredients);
        }

        [Fact]
        public async Task AllShouldReturnCorrectValues()
        {
            await _ingredientsService.CreateRangeAsync(new string[]
            {
                "boost", "primekinit"
            });

            var allIngredients = await _ingredientsService.All();

            Assert.Equal(2, allIngredients.Count);
            Assert.Equal("boost", allIngredients.First().Name);
            Assert.Equal("primekinit", allIngredients.Last().Name);
        }

        [Fact]
        public async Task AnyShouldReturnTrue()
        {
            await _ingredientsService.CreateAsync("boost");

            Assert.True(await _ingredientsService.Any());
        }

        [Fact]
        public async Task AnyShouldReturnFalse()
        {
            Assert.False(await _ingredientsService.Any());
        }
    }
}

