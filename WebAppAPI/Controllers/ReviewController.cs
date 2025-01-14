using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppAPI.Data;
using WebAppAPI.Models;
using WebAppAPI.Models;

namespace WebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _context.Reviews.ToListAsync();

            return Ok(reviews);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetReviewsByRestaurantId(int restaurantId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.RestaurantId == restaurantId)
                .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                return NotFound("No reviews found for this restaurant.");
            }

            var reviewsWithUserDetails = new List<object>();

            foreach (var review in reviews)
            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == review.UserId);

                if (user != null)
                {
                    reviewsWithUserDetails.Add(new
                    {
                        review.Id,
                        review.Comment,
                        review.Rating,
                        User = new
                        {
                            user.Id,
                            user.Username,
                        }
                    });
                }
            }

            return Ok(reviewsWithUserDetails);
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateReview(Reviews review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, Reviews review)
        {
            if (id != review.Id)
                return BadRequest();

            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
