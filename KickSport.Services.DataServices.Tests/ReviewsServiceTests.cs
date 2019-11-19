using AutoMapper;
using KickSport.Data;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Helpers;
using KickSport.Services.DataServices.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KickSport.Services.DataServices.Tests
{
    public class ReviewsServiceTests
    {
        private readonly IGenericRepository<Review> _reviewsRepository;
        private readonly IReviewsService _reviewsService;

        public ReviewsServiceTests()
        {
            var options = new DbContextOptionsBuilder<KickSportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new KickSportDbContext(options);

            var mapperProfile = new MappingConfiguration();
            var conf = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(conf);

            _reviewsRepository = new GenericRepository<Review>(dbContext);
            _reviewsService = new ReviewsService(_reviewsRepository, mapper);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateReviewSuccessfully()
        {
            await _reviewsService.CreateAsync("Very good", "1234", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"));

            Assert.Equal(1, await _reviewsRepository.CountAsync());
            Assert.Equal("Very good", (await _reviewsRepository.FirstAsync()).Text);
            Assert.Equal("1234", (await _reviewsRepository.FirstAsync()).CreatorId);
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"), (await _reviewsRepository.FirstAsync()).ProductId);
        }

        [Fact]
        public async Task GetProductReviewsShouldReturnEmptyCollection()
        {
            var productReviews = await _reviewsService.GetProductReviews(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"));
            Assert.Empty(productReviews);
        }

        [Fact]
        public async Task GetProductReviewsShouldReturnCorrectResults()
        {
            await _reviewsService.CreateAsync("Very good", "1234", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            await _reviewsService.CreateAsync("Brilliant", "12345", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));

            var productReviews = await _reviewsService.GetProductReviews(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));

            Assert.Equal(2, productReviews.Count);
            Assert.Equal("Very good", (await _reviewsRepository.FirstAsync()).Text);
            Assert.Equal("1234", (await _reviewsRepository.FirstAsync()).CreatorId);
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"), (await _reviewsRepository.FirstAsync()).ProductId);
            Assert.Equal("Brilliant", (await _reviewsRepository.LastAsync()).Text);
            Assert.Equal("12345", (await _reviewsRepository.LastAsync()).CreatorId);
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"), (await _reviewsRepository.LastAsync()).ProductId);
        }

        [Fact]
        public async Task DeleteProductReviewsAsyncShouldDeleteProductReviewsSuccessfully()
        {
            await _reviewsService.CreateAsync("Very good", "1234", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            await _reviewsService.CreateAsync("Brilliant", "12345", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));

            var productReviews = await _reviewsService.GetProductReviews(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            Assert.Equal(2, productReviews.Count);

            await _reviewsService.DeleteProductReviewsAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            productReviews = await _reviewsService.GetProductReviews(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            Assert.Empty(productReviews);
        }

        [Fact]
        public async Task DeleteReviewAsyncShouldDeleteReviewSuccessfully()
        {
            await _reviewsService.CreateAsync("Very good", "1234", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            await _reviewsService.CreateAsync("Brilliant", "12345", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));

            var reviewId = (await _reviewsRepository.FirstAsync()).Id;
            await _reviewsService.DeleteReviewAsync(reviewId);

            Assert.Equal(1, await _reviewsRepository.CountAsync());
            Assert.Equal("Brilliant", (await _reviewsRepository.FirstAsync()).Text);
        }

        [Fact]
        public async Task ExistsShouldReturnTrue()
        {
            await _reviewsService.CreateAsync("Very good", "1234", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            await _reviewsService.CreateAsync("Brilliant", "12345", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));

            var firstReviewId = (await _reviewsRepository.FirstAsync()).Id;
            var secondReviewId = (await _reviewsRepository.LastAsync()).Id;

            Assert.True(await _reviewsService.Exists(firstReviewId));
            Assert.True(await _reviewsService.Exists(secondReviewId));
        }

        [Fact]
        public async Task ExistsShouldReturnFalse()
        {
            await _reviewsService.CreateAsync("Very good", "1234", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            await _reviewsService.CreateAsync("Brilliant", "12345", new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));

            Assert.False(await _reviewsService.Exists(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d")));
        }

        [Fact]
        public async Task FindReviewCreatorByIdShouldReturnCorrectReviewCreatorUsername()
        {
            var review = new Review
            {
                Id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"),
                Creator = new ApplicationUser
                {
                    Id = "testId",
                    UserName = "TestUsername"
                },
                ProductId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28e")
            };
            await _reviewsRepository.AddAsync(review);
            await _reviewsRepository.SaveChangesAsync();

            var creatorUsername = _reviewsService.FindReviewCreatorById(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"));
            Assert.Equal("TestUsername", creatorUsername);
        }
    }
}
