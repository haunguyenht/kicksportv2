using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IUsersLikesService
    {
        Task CreateUserLikeAsync(Guid productId, string userId);

        Task DeleteProductLikesAsync(Guid productId);

        Task DeleteUserLikeAsync(Guid productId, string userId);
    }
}
