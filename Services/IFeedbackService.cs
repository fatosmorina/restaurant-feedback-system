using Microsoft.AspNetCore.Mvc;
using RestaurantFeedbackSystem.Dto;

namespace RestaurantFeedbackSystem.Services
{
    public interface IFeedbackService
    {
        IActionResult GetFeedback(string feedbackId);
        IActionResult GetFeedbacks(string restaurantId);
        IActionResult SubmitFeedback(FeedbackDto feedbackDto, string userId);
        IActionResult SubmitResponse(FeedbackDto feedbackDto, string feedbackId, string userId);
    }
}