using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantFeedbackSystem.Dto;
using RestaurantFeedbackSystem.Services;

namespace RestaurantFeedbackSystem.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public IActionResult? CreateRestaurant([FromBody] RestaurantDto restaurantDto)
        {
            string userId = HttpContext.User.FindFirst("userId")?.Value;

            if (userId == null)
                return Unauthorized();

            var result = _restaurantService.CreateRestaurant(restaurantDto, userId);

            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult? GetRestaurant(string id)
        {
            var result = _restaurantService.GetRestaurant(id);

            return result;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult? GetRestaurants()
        {
            var result = _restaurantService.GetRestaurants();

            return result;
        }
    }
}