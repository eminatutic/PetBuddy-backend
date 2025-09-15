using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.ReviewDTO;
using api.Models;

namespace api.Interfaces
{
    public interface IReview
    {

        Task<Review> GetReviewByIdAsync(int id);
        Task<List<Review>> GetAllReviewsAsync();
        Task<List<Review>> GetPendingReviewsByUser(int petId, string userId);
        Task<List<Review>> GetAllPendingReviews();
        Task<Review> ApproveReviewAsync(int reviewId);
        Task<List<Review>> GetReviewsByIdAsync(int petId);
        Task UpdateAverageRatingAsync(int petId);
        Task<Review> CreateReviewAsync(CreateReviewDTO createReviewDTO, string userId);
        Task<bool> DeleteReviewAsync(int id);
        Task<List<Review>> GetReviewsByUserIdAsync(string userId);



    }
}