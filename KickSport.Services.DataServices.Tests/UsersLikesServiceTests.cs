using KickSport.Data;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KickSport.Services.DataServices.Tests
{
    public class UsersLikesServiceTests
    {
        private readonly IGenericRepository<UsersLikes> _usersLikesRepository;
        private readonly UsersLikesService _usersLikesService;

        public UsersLikesServiceTests()
        {
            var options = new DbContextOptionsBuilder<KickSportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new KickSportDbContext(options);

            _usersLikesRepository = new GenericRepository<UsersLikes>(dbContext);
            _usersLikesService = new UsersLikesService(_usersLikesRepository);
        }

        [Fact]
        public async Task CreateUserLikeAsyncShouldCreateUserLikeSuccessfully()
        {
            await _usersLikesService.CreateUserLikeAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"), "abcd");
            Assert.Equal(1, await _usersLikesRepository.CountAsync());

            var userLike = await _usersLikesRepository.FirstAsync();
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"), userLike.ProductId);
            Assert.Equal("abcd", userLike.ApplicationUserId);
        }

        [Fact]
        public async Task DeleteProductLikesAsyncShouldDeleteProductLikesSuccessfully()
        {
            await _usersLikesService.CreateUserLikeAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"), "abcd");
            await _usersLikesService.CreateUserLikeAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"), "abcde");
            await _usersLikesService.CreateUserLikeAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"), "abcd");

            await _usersLikesService.DeleteProductLikesAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"));

            Assert.Equal(1, await _usersLikesRepository.CountAsync());
            var userLike = await _usersLikesRepository.FirstAsync();
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"), userLike.ProductId);
        }

        [Fact]
        public async Task DeleteUserLikeAsyncShouldDeleteUserLikesSuccessfully()
        {
            await _usersLikesService.CreateUserLikeAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"), "abcd");
            await _usersLikesService.CreateUserLikeAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"), "abcd");

            await _usersLikesService.DeleteUserLikeAsync(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c"), "abcd");

            Assert.Equal(1, await _usersLikesRepository.CountAsync());
            var userLike = await _usersLikesRepository.FirstAsync();
            Assert.Equal(new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28d"), userLike.ProductId);
        }
    }
}
