namespace RestaurantFeedbackSystem.Models;

public class Feedback
{
    public string Id { get; set; }
    public double Rating { get; set; }
    public string RestaurantId { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Comment { get; set; }
    
    public virtual Restaurant Restaurant { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
    
    public string? ParentFeedbackId { get; set; }
    public virtual Feedback? ParentFeedback { get; set; }
}