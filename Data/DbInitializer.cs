using RestaurantFeedbackSystem.Models;

namespace RestaurantFeedbackSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(RestaurantDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Restaurants.Any())
            {
                return;
            }

            var superUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Super Admin",
                Email = "super.admin@gmail.com",
                Role = Role.Admin
            };
            superUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("strongpassword789");
            context.Users.Add(superUser);

            var user1 = CreateUser("John Doe", "john.doe@example.com", "strongpassword123", Role.Customer, context);
            var user2 = CreateUser("Jane Smith", "jane.smith@example.com", "strongpassword456", Role.Customer, context);

            var restaurant1 = CreateRestaurant("Fantastic Grill", "123 Main St", user1, context);
            var restaurant2 = CreateRestaurant("Tasty Bites", "456 Oak St", user2, context);

            var feedback1 = CreateFeedback(5, "Great food!", restaurant1, user1, context);

            var parentFeedback = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                Comment = "Thank you for your feedback!",
                Date = DateTime.Now,
                Restaurant = restaurant2,
                User = user2
            };

            var feedback2 = CreateFeedback(4, "Good service.", restaurant2, user2, context);
            feedback2.ParentFeedback = parentFeedback;
            feedback2.ParentFeedbackId = parentFeedback.Id;

            context.Feedbacks.AddRange(feedback1, feedback2, parentFeedback);
            context.Restaurants.AddRange(restaurant1, restaurant2);

            context.SaveChanges();
        }

        private static User CreateUser(string name, string email, string password, Role role, RestaurantDbContext context)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Email = email,
                Role = role
            };
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            context.Users.Add(user);
            return user;
        }

        private static Restaurant CreateRestaurant(string name, string location, User user, RestaurantDbContext context)
        {
            var restaurant = new Restaurant
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Location = location,
                User = user
            };
            context.Restaurants.Add(restaurant);
            return restaurant;
        }

        private static Feedback CreateFeedback(int rating, string comment, Restaurant restaurant, User user, RestaurantDbContext context)
        {
            var feedback = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                Rating = rating,
                Comment = comment,
                Restaurant = restaurant,
                User = user
            };
            context.Feedbacks.Add(feedback);
            return feedback;
        }
    }
}
