using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices
{
    public class UsersLikesService : IUsersLikesService
    {
        private readonly IGenericRepository<UsersLikes> _usersLikesRepository;

        public UsersLikesService(IGenericRepository<UsersLikes> usersLikesRepository)
        {
            _usersLikesRepository = usersLikesRepository;
        }

        public async Task CreateUserLikeAsync(string productId, string userId)
        {
            await _usersLikesRepository.AddAsync(new UsersLikes
            {
                ApplicationUserId = userId,
                ProductId = productId
            });

            await _usersLikesRepository.SaveChangesAsync();
        }

        public async Task DeleteProductLikesAsync(string productId)
        {
            var productLikes = _usersLikesRepository
                .DbSet
                .Where(ul => ul.ProductId == productId)
                .ToList();

            if (productLikes.Any())
            {
                await _usersLikesRepository.DeleteRange(productLikes);
                await _usersLikesRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteUserLikeAsync(string productId, string userId)
        {
            var userLike = _usersLikesRepository
                .DbSet
                .FirstOrDefault(ul => ul.ApplicationUserId == userId && ul.ProductId == productId);

            if (userLike != null)
            {
                await _usersLikesRepository.Delete(userLike);
                await _usersLikesRepository.SaveChangesAsync();
            }
        }
    }
}
