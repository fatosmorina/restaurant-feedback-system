using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantFeedbackSystem.Data;
using RestaurantFeedbackSystem.Dto;
using RestaurantFeedbackSystem.Models;

namespace RestaurantFeedbackSystem.Controllers;

[ApiController]
[Route("api")]
public class FeedbackController : ControllerBase
{
    private readonly RestaurantDbContext _restaurantContext;

    public FeedbackController(RestaurantDbContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    [HttpGet("feedbacks/{feedbackId}"), Authorize(Roles = "Admin,Customer")]
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
            return NotFound();

        return Ok(response);
    }

    [HttpGet("restaurants/{restaurantId}/feedbacks"), Authorize(Roles = "Admin,Customer")]
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

        return Ok(response);
    }

    [HttpPost("feedbacks"), Authorize(Roles = "Customer")]
    public IActionResult SubmitFeedback([FromBody] FeedbackDto feedbackDto)
    {
        var userId = HttpContext.User.FindFirst("userId")?.Value;
        
        if (userId == null)
            return Unauthorized();
        
        var restaurant = _restaurantContext.Restaurants
            .Include(r => r.FeedbackList)
            .FirstOrDefault(r => r.Id == feedbackDto.RestaurantId);
        
        if (restaurant == null)
            return NotFound(new { Message = "Restaurant not found" });
        
        if (feedbackDto.Rating is < 1 or > 5)
            return BadRequest(new { Message = "Rating must be between 1 and 5" });
        
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
        
        return Ok(new { FeedbackId = feedback.Id, Message = "Feedback submitted successfully" });
    }
    
    [HttpPost("feedbacks/{feedbackId}/responses"), Authorize(Roles = "Admin")]
    public IActionResult SubmitResponse(string feedbackId, [FromBody] FeedbackDto feedbackDto)
    {
        
        var userId = HttpContext.User.FindFirst("userId")?.Value;
        if (userId == null)
            return Unauthorized();
        
        var feedback = _restaurantContext.Feedbacks.FirstOrDefault(f => f.Id == feedbackId);
        
        if (feedback == null)
            return NotFound(new { Message = "Feedback not found" });

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
        
        return Ok(new { ResponseId = response.Id, Message = "Response submitted successfully" });
    }
}
