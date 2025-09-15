using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Data;
using System.Security.AccessControl;
using api.Interfaces;
using AutoMapper;
using api.DTOs.ReviewDTO;
using System.Drawing;

namespace api.Repositories
{
    public class ReviewRepository : IReview
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ReviewRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //by id
        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                                 .Include(r => r.User)
                                 .FirstOrDefaultAsync(r => r.Id == id);
        }


        //by userId
        public async Task<List<Review>> GetReviewsByUserIdAsync(string userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && r.Pet.IsDeleted == false)
                .Include(r => r.Pet)
                .ToListAsync();
        }


        //pending reviews by user
        public async Task<List<Review>> GetPendingReviewsByUser(int petId, string userId)
        {
            return await _context.Reviews
                .Where(r => r.PetId == petId && r.UserId == userId && r.Status == "Pending")
                .ToListAsync();
        }


        //all reviews
        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                 .Include(r => r.User)
                 .Include(r => r.Pet)
                 .Where(r => r.Pet.IsDeleted == false && r.User.isDeleted == false)
                 .ToListAsync();
        }



        //all pending reviews for admin
        public async Task<List<Review>> GetAllPendingReviews()
        {
            return await _context.Reviews
                .Where(r => r.Status == "Pending" && r.Pet.IsDeleted == false && r.User.isDeleted == false)
                .Include(r => r.User)
                .Include(r => r.Pet)
                .ToListAsync();
        }


        //approve 
        public async Task<Review> ApproveReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return null;

            review.Status = "Approved";
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            await UpdateAverageRatingAsync(review.PetId);
            return review;
        }


        //reviews by petId
        public async Task<List<Review>> GetReviewsByIdAsync(int petId)
        {
            return await _context.Reviews
                .Where(r => r.PetId == petId && r.Status == "Approved" && r.User.isDeleted == false)
                .Where(r => r.Status == "Approved")
                .Include(r => r.User)
                .ToListAsync();
        }



        //update averagerating
        public async Task UpdateAverageRatingAsync(int petId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null) return;

            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.PetId == petId && r.Status == "Approved" && r.User.isDeleted == false)
                .Select(r => r.Rating)
                .ToListAsync();

            pet.AverageRating = reviews.Any() ? (float)reviews.Average() : 0f;

            await _context.SaveChangesAsync();
        }


        //add 
        public async Task<Review> CreateReviewAsync(CreateReviewDTO createReviewDTO, string userId)
        {
            var userExists = await _context.Users.FindAsync(userId);
            if (userExists == null)
            {
                return null;
            }

            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == createReviewDTO.PetId);
            if (pet == null)
            {
                return null;
            }

            var review = _mapper.Map<Review>(createReviewDTO);
            review.UserId = userId;
            if (review.Rating == 0)
            {
                review.Rating = null;
            }

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return review;
        }




        //delete
        public async Task<bool> DeleteReviewAsync(int id)
        {
            var deleteReview = await _context.Reviews.FindAsync(id);

            if (deleteReview != null)
            {
                int petId = deleteReview.PetId;

                _context.Reviews.Remove(deleteReview);
                await _context.SaveChangesAsync();

                await UpdateAverageRatingAsync(petId);

                return true;
            }

            return false;
        }



    }
}