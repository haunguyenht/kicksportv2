using KickSport.Services.DataServices.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IReviewsService
    {
        Task<ReviewDto> CreateAsync(string text, string creatorId, string productId);

        IEnumerable<ReviewDto> GetProductReviews(string productId);

        Task DeleteProductReviewsAsync(string productId);

        Task DeleteReviewAsync(string reviewId);

        Task<bool> Exists(string reviewId);

        string FindReviewCreatorById(string reviewId);
    }
}
