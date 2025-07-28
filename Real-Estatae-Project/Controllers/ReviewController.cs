using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO.Review;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        IReviewRepository reviewRepository;
        IUserRepository userRepository;

        public ReviewController(IReviewRepository _reviewRepository, IUserRepository _userRepository)
        {
            reviewRepository = _reviewRepository;
            userRepository = _userRepository;
        }


        #region Add review : renter only
        [HttpPost]

        public async Task<IActionResult> AddReview([FromForm] ReviewDTO reviewDTO)

        {
            string renterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool hasCommunity = await reviewRepository.CheckIfRenterHasCommunity(renterId);
            if (string.IsNullOrEmpty(renterId) || !User.IsInRole("Renter") || hasCommunity == false)
            {
                return Unauthorized(new { message = "Unauthorized access." });
            }

            int? communityID = await userRepository.GetCommunityId(renterId, "Renter");
            int communityId = communityID ?? 0;

            if(communityId==0 || communityId==null)
            {
                  return Unauthorized(new { message = "You don't have a community" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Review addedReview = new Review
            {
                rate = reviewDTO.rate,
                content = reviewDTO.content,
                userId = renterId,
                communityId = communityId,
                isDeleted = false
            };

            var result = await reviewRepository.Add(addedReview);
            if (result == null)
            {
                return BadRequest(new { message = "Failed to add review." });
            }
            return Ok(new { message = "Review added successfully.", reviewId = addedReview.id, communityId = addedReview.communityId });
        }
        #endregion

        #region edit a review renter only
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ReviewDTO reviewDTO)
        {
            string renterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool hasCommunity = await reviewRepository.CheckIfRenterHasCommunity(renterId);

            if (string.IsNullOrEmpty(renterId) || !User.IsInRole("Renter") || hasCommunity == false)
            {
                return Unauthorized(new { message = "Unauthorized access." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool updated = await reviewRepository.Edit(id, reviewDTO, renterId);

            if (!updated)
            {
                return NotFound(new { message = "Review not found or you do not have permission to edit it." });
            }

            return Ok(new { message = "Review updated successfully." });

        }

        #endregion

        #region Delete a review renter only
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            string renterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool hasCommunity = await reviewRepository.CheckIfRenterHasCommunity(renterId);

            if (string.IsNullOrEmpty(renterId) || !User.IsInRole("Renter") || hasCommunity == false)
            {
                return Unauthorized(new { message = "Unauthorized access." });
            }
            bool deleted = await reviewRepository.Delete(id, renterId);
            if (!deleted)
            {
                return NotFound(new { message = "Review not found or you do not have permission to delete it." });
            }
            return Ok(new { message = "Review deleted successfully." });
        }
        #endregion

        #region Get all reviews by communityId
        [HttpGet("communityReviews/{communityId:int}")]
        public async Task<IActionResult> GetAllReviews(int communityId)
        {
            if (communityId <= 0)
            {
                return BadRequest(new { message = "Invalid community ID." });
            }
            List<Review> reviews = await reviewRepository.GetAll(communityId);
            if (reviews == null || reviews.Count == 0)
            {
                return NotFound(new { message = "No reviews found for this community." });
            }
            return Ok(reviews);
        }

        #endregion

        #region Get all reviews of all communities
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            List<AllReviewDTO> reviews = await reviewRepository.GetAllReviews();
            if (reviews == null || reviews.Count == 0)
            {
                return NotFound(new { message = "No reviews found." });
            }
            return Ok(reviews);
        }
        #endregion


        #region get all reviews of a renter 
        [HttpGet("renterReviews")]
        public async Task<IActionResult> GetAllReviewsByRenterId()
        {
            string renterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(renterId) || !User.IsInRole("Renter"))
            {
                return Unauthorized(new { message = "Unauthorized access." });
            }
            List<Review> reviews = await reviewRepository.GetAllReviewsByRenterId(renterId);
            if (reviews == null || reviews.Count == 0)
            {
                return NotFound(new { message = "No reviews found for this renter." });
            }
            return Ok(reviews);
        }
        #endregion


    }
}
