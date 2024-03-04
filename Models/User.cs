using Newtonsoft.Json;

namespace RestaurantFeedbackSystem.Models;

public class User
{
    public User(string email) { Email = email;}
    
    public User(string id, string name, string email, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        Role = role;
    }
   
    public User() { }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
    public Role Role { get; set; }
    public List<Feedback>? FeedbackList { get; set; } = new List<Feedback>();
    public List<Restaurant>? Restaurants { get; set; } = new List<Restaurant>();
    [JsonIgnore]
    public virtual List<UserRestaurant> UserRestaurants { get; set; } = new List<UserRestaurant>();
}