namespace RestaurantFeedbackSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<UserRestaurant> UserRestaurants { get; set; }


        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }
    }
}