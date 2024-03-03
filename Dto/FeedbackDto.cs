namespace RestaurantFeedbackSystem.Dto
{
    public class FeedbackDto
    {
        public int Rating { get; set; }
        public string RestaurantId { get; set; }
        
        public string Comment { get; set; }
        
        public FeedbackDto()
        {
        }

        public FeedbackDto(int rating, string restaurantId)
        {
            Rating = rating;
            RestaurantId = restaurantId;
        }
    }
}