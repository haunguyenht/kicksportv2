using KickSport.Services.DataServices.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IReviewsService
    {
        Task<ReviewDto> CreateAsync(string text, string creatorId, Guid productId);

        Task<List<ReviewDto>> GetProductReviews(Guid productId);

        Task DeleteProductReviewsAsync(Guid productId);

        Task DeleteReviewAsync(Guid reviewId);

        Task<bool> Exists(Guid reviewId);

        string FindReviewCreatorById(Guid reviewId);
    }
}
