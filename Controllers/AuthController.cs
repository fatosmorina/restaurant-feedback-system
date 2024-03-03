using Microsoft.AspNetCore.Mvc;
using RestaurantFeedbackSystem.Data;
using RestaurantFeedbackSystem.Dto;
using RestaurantFeedbackSystem.Services;

namespace RestaurantFeedbackSystem.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(RestaurantDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("auth")]
        public IActionResult Login(UserDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            string token = _jwtTokenService.GenerateToken(user);

            return Ok(new { Token = token});
        }
    }
}