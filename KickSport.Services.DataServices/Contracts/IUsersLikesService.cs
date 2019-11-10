using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IUsersLikesService
    {
        Task CreateUserLikeAsync(string productId, string userId);

        Task DeleteProductLikesAsync(string productId);

        Task DeleteUserLikeAsync(string productId, string userId);
    }
}
