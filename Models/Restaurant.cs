using Newtonsoft.Json;

namespace RestaurantFeedbackSystem.Models;

public class Restaurant
{
    public Restaurant(string id, string userId, string name, string location)
    { 
        Id = id;
        Name = name;
        Location = location;
        UserId = userId;
    }

    public Restaurant()
    {
        
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public virtual List<Feedback> FeedbackList { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
}