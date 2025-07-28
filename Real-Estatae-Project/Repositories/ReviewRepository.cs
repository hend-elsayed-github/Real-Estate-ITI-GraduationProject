using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Review;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class ReviewRepository: IReviewRepository
    {
        private readonly ProjectContext _context;

        private readonly UserManager<ApplicationUser> userManager;
        public ReviewRepository(ProjectContext context, UserManager<ApplicationUser> _userManager)
        {
            _context = context;
            userManager = _userManager;
        }


        #region Add a review

        public async Task<Review> Add(Review newReview)
        {
            if (newReview == null)
            {
                return null;
            }
            await _context.Reviews.AddAsync(newReview);

            await _context.SaveChangesAsync();

            return newReview;
        }
        #endregion

        #region check if the renter has community or not, if he has so he can add review otherwise he can't :D
        public async Task<bool> CheckIfRenterHasCommunity(string renterId)
        {
            if (renterId == null)
            {
                return false; // Renter ID is null
            }
            // Check if the renter has any communities
            bool hasCommunity = await userManager.Users
                .AnyAsync(u => u.communityId != null && u.Id == renterId);
            return hasCommunity; // Return true if the renter has at least one community
        }
        #endregion


        #region edit a review
        public async Task<bool> Edit(int id, ReviewDTO updatedReview, string renterId)
        {
            Review existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null || existingReview.userId != renterId)
            {
                return false; // Review not found or user does not have permission to edit
            }

            if (existingReview == null)
            {
                return false; // Review not found
            }
            // Update the properties of the existing review
            existingReview.rate = updatedReview.rate;
            existingReview.content = updatedReview.content;
            await _context.SaveChangesAsync();
            return true; // Successfully updated
        }
        #endregion


        #region Delete a review

        public async Task<bool> Delete(int id, string renterId)
        {
            Review review = await _context.Reviews
                .Where(r => r.id == id && r.userId == renterId)
                .FirstOrDefaultAsync();

            if (review == null)
            {
                return false; // Review not found
            }
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true; // Successfully deleted
        }

        #endregion


        #region Get all reviews of a community by community ID

        public async Task<List<Review>> GetAll(int _communityId)
        {
            return await _context.Reviews
                .Include(r => r.community)
                .Where(r => r.communityId == _communityId)
                .ToListAsync();
        }
        #endregion


        #region get review by id 
        public async Task<Review> GetById(int id)
        {
            return await _context.Reviews
                .Include(r => r.community)
                .FirstOrDefaultAsync(r => r.id == id);
        }
        #endregion


        #region get a review by id and by community id
        public async Task<Review> GetByIdAndCommunityId(int id, int communityId)
        {
            return await _context.Reviews
                .Include(r => r.community)
                .FirstOrDefaultAsync(r => r.id == id && r.communityId == communityId);
        }

        #endregion


        #region get all reviews of all communites
        public async Task<List<AllReviewDTO>> GetAllReviews()
        {
            var Reviews = await _context.Reviews
                .Include(r => r.community)
                .Include(r => r.renter)
                .ToListAsync();
            var result = Reviews.Select(r => new AllReviewDTO
            {
                id = r.id,
                content = r.content,
                publishDate = r.publishDate,
                fullName = r.renter.firstName + ' ' + r.renter.lastName,
                userImage = r.renter.image,
                userName = r.renter.UserName,
                rate = r.rate
            }).ToList();

            return result;


        }
        #endregion


        #region get all reviews of a renter 
        public async Task<List<Review>> GetAllReviewsByRenterId(string renterId)
        {
            return await _context.Reviews
                .Include(r => r.community)
                .Where(r => r.userId == renterId)
                .ToListAsync();
        }
        #endregion
    }
}
