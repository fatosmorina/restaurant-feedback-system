using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantFeedbackSystem.Data;
using RestaurantFeedbackSystem.Dto;
using System;
using System.Linq;
using RestaurantFeedbackSystem.Models;

namespace RestaurantFeedbackSystem.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly RestaurantDbContext _restaurantContext;

        public FeedbackService(RestaurantDbContext restaurantContext)
        {
            _restaurantContext = restaurantContext;
        }

        public IActionResult GetFeedback(string feedbackId)
        {
            var response = _restaurantContext.Feedbacks
                .Where(f => f.Id == feedbackId)
                .Select(f => new
                {
                    f.Id,
                    f.Rating,
                    f.RestaurantId,
                    f.Date,
                    f.Comment,
                })
                .FirstOrDefault();

            if (response == null)
                return new NotFoundResult();

            return new OkObjectResult(response);
        }

        public IActionResult GetFeedbacks(string restaurantId)
        {
            var response = _restaurantContext.Feedbacks
                .Where(f => f.RestaurantId == restaurantId)
                .Select(f => new
                {
                    f.Id,
                    f.Rating,
                    f.RestaurantId,
                    f.Date,
                    f.Comment,
                })
                .ToList();

            return new OkObjectResult(response);
        }

        public IActionResult SubmitFeedback(FeedbackDto feedbackDto, string userId)
        {
            var restaurant = _restaurantContext.Restaurants
                .Include(r => r.FeedbackList)
                .FirstOrDefault(r => r.Id == feedbackDto.RestaurantId);
            
            if (restaurant == null)
                return new NotFoundObjectResult(new { Message = "Restaurant not found" });
            
            if (feedbackDto.Rating is < 1 or > 5)
                return new BadRequestObjectResult(new { Message = "Rating must be between 1 and 5" });
            
            var feedback = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                Rating = feedbackDto.Rating,
                RestaurantId = feedbackDto.RestaurantId,
                UserId = userId,
                Comment = feedbackDto.Comment,
            };

            _restaurantContext.Add(feedback);
            _restaurantContext.SaveChanges();
            
            return new OkObjectResult(new { FeedbackId = feedback.Id, Message = "Feedback submitted successfully" });
        }

        public IActionResult SubmitResponse(FeedbackDto feedbackDto, string feedbackId, string userId)
        {
        
            var isAdminForRestaurant = _restaurantContext.UserRestaurants
                .Any(ur => ur.UserId == userId && ur.RestaurantId == feedbackDto.RestaurantId && ur.User.Role == Role.Admin);

            if (!isAdminForRestaurant)
                return new ForbidResult();
        
            var feedback = _restaurantContext.Feedbacks.FirstOrDefault(f => f.Id == feedbackId);
        
            if (feedback == null)
                return new NotFoundObjectResult(new { Message = "Feedback not found" });

            var response = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                Rating = feedbackDto.Rating,
                RestaurantId = feedbackDto.RestaurantId,
                UserId = userId,
                ParentFeedbackId = feedback.Id,
                Comment = feedbackDto.Comment,
            };

            _restaurantContext.Add(response);
            _restaurantContext.SaveChanges();
        
            return new OkObjectResult(new { ResponseId = response.Id, Message = "Response submitted successfully" });
        }
    }
}
