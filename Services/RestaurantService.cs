using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantFeedbackSystem.Data;
using RestaurantFeedbackSystem.Dto;
using RestaurantFeedbackSystem.Models;

namespace RestaurantFeedbackSystem.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _context;

        public RestaurantService(RestaurantDbContext context)
        {
            _context = context;
        }

        public IActionResult CreateRestaurant(RestaurantDto restaurantDto, string userId)
        {
            var restaurant = new Restaurant(Guid.NewGuid().ToString(), userId, restaurantDto.Name, restaurantDto.Location);
            _context.Restaurants.Add(restaurant);

            var userRestaurant = new UserRestaurant
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                RestaurantId = restaurant.Id
            };

            _context.UserRestaurants.Add(userRestaurant);
            _context.SaveChanges();

            return new OkObjectResult(restaurant);
        }

        public IActionResult GetRestaurant(string id)
        {
            var restaurant = _context.Restaurants
                .Include(r => r.FeedbackList)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
                return new NotFoundResult();

            var overallRating = restaurant.FeedbackList.Count > 0 ? restaurant.FeedbackList.Average(f => f.Rating) : 0;

            var result = new
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location,
                OverallRating = overallRating
            };

            return new OkObjectResult(result);
        }

        public IActionResult GetRestaurants()
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

            return new OkObjectResult(restaurants);
        }
    }
}
