using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Models;
using api.DTOs.ReviewDTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReview _reviewRepository;

        public ReviewController(IReview reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }


        //all reviews
        [HttpGet("all-reviews")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync();
            return Ok(reviews);
        }


        //pending reviews
        [HttpGet("pending-reviews")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingReviews()
        {
            var pendingReviews = await _reviewRepository.GetAllPendingReviews();

            return Ok(pendingReviews);
        }


        //pending reviews by userId
        [HttpGet("pending-reviews-for-user/{petId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPendingReviewsForUser(int petId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Review> pendingReviews = await _reviewRepository.GetPendingReviewsByUser(petId, userId);

            return Ok(pendingReviews);
        }



        //by userId
        [HttpGet("get-reviews-by-userId/{userId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetReviewsByUserId(string userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }


        //by petId
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetReview([FromRoute] int petId)
        {
            var reviews = await _reviewRepository.GetReviewsByIdAsync(petId);


            var reviewsWithUserInfo = reviews.Select(r => new
            {
                r.UserId,
                r.Id,
                r.PetId,
                r.Comment,
                r.Rating,
                CTime = r.CTime,
                UserName = r.User.UserName
            }).ToList();

            return Ok(reviewsWithUserInfo);
        }

        //add
        [HttpPost("create-review")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDTO createReviewDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = await _reviewRepository.CreateReviewAsync(createReviewDTO, userId);

            if (review == null)
            {
                return BadRequest("Failed to create review.");
            }
            else
            {
                return CreatedAtAction(nameof(GetReview), new { petId = review.PetId }, review);
            }
        }


        //approve
        [HttpPost("approve-review/{reviewId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveReview(int reviewId)
        {
            var updatedReview = await _reviewRepository.ApproveReviewAsync(reviewId);

            if (updatedReview == null)
            {
                return BadRequest("Failed to approve review.");
            }
            else
            {
                return Ok(updatedReview);
            }
        }

        //delete
        [HttpDelete("delete-review/{reviewId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {

            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);

            if (review == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var deleteReview = await _reviewRepository.DeleteReviewAsync(reviewId);
            if (deleteReview)
                return NoContent();

            return NotFound();
        }

    }
}