using AutoMapper;
using KickSport.Data;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KickSport.Services.DataServices.Tests
{
    public class CategoriesServiceTests
    {
        private readonly IGenericRepository<Category> _categoriesRepository;
        private readonly CategoriesService _categoriesService;

        public CategoriesServiceTests()
        {
            var options = new DbContextOptionsBuilder<KickSportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new KickSportDbContext(options);

            var mapperProfile = new MappingConfiguration();
            var conf = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(conf);

            _categoriesRepository = new GenericRepository<Category>(dbContext);
            _categoriesService = new CategoriesService(_categoriesRepository, mapper);
        }
        [Fact]
        public async Task CreateAsyncShouldCreateCategorySuccessfully()
        {
            await _categoriesService.CreateAsync("Nike");

            Assert.Equal(1,await _categoriesRepository.CountAsync());
            Assert.Equal("Nike", (await _categoriesRepository.FirstAsync()).Name);
        }

        [Fact]
        public async Task CreateRangeAsyncShouldCreateCategorySuccessfully()
        {
            await _categoriesService.CreateRangeAsync(new string[]
            {
                "Nike", "Adidas"
            });

            Assert.Equal(2, await _categoriesRepository.CountAsync());
            Assert.Equal("Nike", (await _categoriesRepository.FirstAsync()).Name);
            Assert.Equal("Adidas", (await _categoriesRepository.LastAsync()).Name);
        }

        [Fact]
        public async Task FindByNameShouldReturnNull()
        {
            var category = await _categoriesService.FindByName("Nike");
            Assert.Null(category);
        }

        [Fact]
        public async Task FindByNameShouldReturnCorrectValues()
        {
            await _categoriesService.CreateAsync("Nike");

            var category = await _categoriesService.FindByName("Nike");
            Assert.Equal("Nike", category.Name);
        }

        [Fact]
        public async Task AllShouldReturnEmptyCollection()
        {
            var allCategories = await _categoriesService.All();
            Assert.Empty(allCategories);
        }

        [Fact]
        public async Task AllShouldReturnCorrectValues()
        {
            await _categoriesService.CreateRangeAsync(new string[]
            {
                "Nike", "Adidas"
            });

            var allCategories = await _categoriesService.All();
            Assert.Equal(2, allCategories.Count);
        }

        [Fact]
        public async Task AnyShouldReturnTrue()
        {
            await _categoriesService.CreateAsync("Nike");

            Assert.True(await _categoriesService.Any());
        }

        [Fact]
        public async Task AnyShouldReturnFalse()
        {
            Assert.False(await _categoriesService.Any());
        }
    }
}
