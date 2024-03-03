using RestaurantFeedbackSystem.Models;

namespace RestaurantFeedbackSystem.Dto
{
    public class RestaurantDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        
        public RestaurantDto(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}