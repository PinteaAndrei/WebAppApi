using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppAPI.Data;
using WebAppAPI.Models;

namespace WebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();  
            }

            return Ok(user);  
        }
    
        
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            return Ok(new { exists = emailExists });
        }

        [HttpGet("check-username")]
        public async Task<IActionResult> CheckUsername(string username)
        {
            var usernameExists = await _context.Users.AnyAsync(u => u.Username == username);
            return Ok(new { exists = usernameExists });
        }
        
        [HttpGet("check-password")]
        public async Task<IActionResult> CheckPassword(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

           
            if (user.Pass == password)
            {
                return Ok(new { exists = true , userId = user.Id });
            }

            return Unauthorized("Incorrect password");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}