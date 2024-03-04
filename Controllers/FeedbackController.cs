using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantFeedbackSystem.Dto;
using RestaurantFeedbackSystem.Services;

namespace RestaurantFeedbackSystem.Controllers
{
    [ApiController]
    [Route("api")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("feedbacks/{feedbackId}"), Authorize(Roles = "Admin,Customer")]
        public IActionResult GetFeedback(string feedbackId)
        {
            return _feedbackService.GetFeedback(feedbackId);
        }

        [HttpGet("restaurants/{restaurantId}/feedbacks"), Authorize(Roles = "Admin,Customer")]
        public IActionResult GetFeedbacks(string restaurantId)
        {
            return _feedbackService.GetFeedbacks(restaurantId);
        }

        [HttpPost("feedbacks"), Authorize(Roles = "Customer")]
        public IActionResult SubmitFeedback([FromBody] FeedbackDto feedbackDto)
        {
            var userId = HttpContext.User.FindFirst("userId")?.Value;
            return _feedbackService.SubmitFeedback(feedbackDto, userId);
        }

        [HttpPost("feedbacks/{feedbackId}/responses"), Authorize(Roles = "Admin")]
        public IActionResult SubmitResponse(string feedbackId, [FromBody] FeedbackDto feedbackDto)
        {
            var userId = HttpContext.User.FindFirst("userId")?.Value;
            return _feedbackService.SubmitResponse(feedbackDto, feedbackId, userId);
        }
    }
}