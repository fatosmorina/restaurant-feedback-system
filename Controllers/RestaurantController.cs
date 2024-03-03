using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantFeedbackSystem.Data;
using RestaurantFeedbackSystem.Dto;
using RestaurantFeedbackSystem.Models;

namespace RestaurantFeedbackSystem.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantController : ControllerBase
{
    private readonly RestaurantDbContext _context;
    
    public RestaurantController(RestaurantDbContext context)
    {
        _context = context;
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public IActionResult? CreateRestaurant([FromBody] RestaurantDto restaurantDto)
    {
        string userId = HttpContext.User.FindFirst("userId")?.Value;
        
        if (userId == null)
            return Unauthorized();
        
        var restaurant = new Restaurant(Guid.NewGuid().ToString(), userId, restaurantDto.Name, restaurantDto.Location);
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();
        return Ok(restaurant);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Customer")]
    public IActionResult? GetRestaurant(string id)
    {
        var restaurant = _context.Restaurants
            .Include(r => r.FeedbackList)
            .FirstOrDefault(r => r.Id == id);

        if (restaurant == null)
            return NotFound();

        var overallRating = restaurant.FeedbackList.Count > 0 ? restaurant.FeedbackList.Average(f => f.Rating) : 0;

        var result = new
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Location = restaurant.Location,
            OverallRating = overallRating
        };

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    public IActionResult? GetRestaurants()
    {
        var restaurants = _context.Restaurants
            .Include(r => r.FeedbackList)
            .Select(r => new
            {
                Id = r.Id,
                Name = r.Name,
                Location = r.Location,
                OverallRating = r.FeedbackList.Count > 0 ? r.FeedbackList.Average(f => f.Rating) : 0
            })
            .ToList();

        return Ok(restaurants);
    }
}