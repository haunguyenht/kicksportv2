using AutoMapper;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices
{
    public class ReviewsService : IReviewsService
    {
        private readonly IGenericRepository<Review> _reviewsRepository;
        private readonly IMapper _mapper;

        public ReviewsService(
            IGenericRepository<Review> reviewsRepository,
            IMapper mapper)
        {
            _reviewsRepository = reviewsRepository;
            _mapper = mapper;
        }

        public async Task<ReviewDto> CreateAsync(string text, string creatorId, string productId)
        {
            var review = new Review
            {
                Text = text,
                CreatorId = creatorId,
                ProductId = productId,
                LastModified = DateTime.Now
            };

            await _reviewsRepository.AddAsync(review);
            await _reviewsRepository.SaveChangesAsync();

            var result = _mapper.Map<ReviewDto>(review);

            return result;
        }

        public IEnumerable<ReviewDto> GetProductReviews(string productId)
        {
            var productReviews = _reviewsRepository
                .DbSet
                .Include(r => r.Creator)
                .Where(r => r.ProductId == productId).ToListAsync();
            var reviewDto = _mapper.Map<IEnumerable<ReviewDto>>(productReviews).ToList();
            return reviewDto;
        }

        public async Task DeleteProductReviewsAsync(string productId)
        {
            var reviews = _reviewsRepository
                .DbSet
                .Where(r => r.ProductId == productId)
                .ToList();

            if (reviews.Any())
            {
                _reviewsRepository.DeleteRange(reviews);
                await _reviewsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteReviewAsync(string reviewId)
        {
            var review = _reviewsRepository
                .DbSet
                .First(r => r.Id == reviewId);

            _reviewsRepository.Delete(review);
            await _reviewsRepository.SaveChangesAsync();
        }

        public async Task<bool> Exists(string reviewId)
        {
            var review = await _reviewsRepository.FindOneAsync(r => r.Id == reviewId);

            return review != null;
        }

        public string FindReviewCreatorById(string reviewId)
        {
            return _reviewsRepository
                .DbSet
                .Include(r => r.Creator)
                .First(r => r.Id == reviewId)
                .Creator
                .UserName;
        }
    }
}