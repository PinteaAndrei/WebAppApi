using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppAPI.Data;
using WebAppAPI.Models;

namespace WebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RestaurantsController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> SearchRestaurants(string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Search query cannot be empty");

            
            var restaurants = await _context.Restaurants
                .Where(r => r.RestName.Contains(query) || r.RestAddress.Contains(query))  
                .ToListAsync();

            if (restaurants == null || restaurants.Count == 0)
                return NotFound("No restaurants found");

            return Ok(restaurants);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant(Restaurants restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, restaurant);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, Restaurants restaurant)
        {
            if (id != restaurant.Id)
                return BadRequest();

            _context.Entry(restaurant).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return NotFound();

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
