namespace RestaurantFeedbackSystem.Models
{
    public class UserRestaurant
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        
        public string RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
