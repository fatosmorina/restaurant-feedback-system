using Microsoft.AspNetCore.Mvc;
using RestaurantFeedbackSystem.Dto;

namespace RestaurantFeedbackSystem.Services
{
    public interface IRestaurantService
    {
        IActionResult CreateRestaurant(RestaurantDto restaurantDto, string userId);
        IActionResult GetRestaurant(string id);
        IActionResult GetRestaurants();
    }
}