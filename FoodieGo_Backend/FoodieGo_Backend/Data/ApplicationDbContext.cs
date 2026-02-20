using Microsoft.EntityFrameworkCore;
using FoodieGo_Backend.Data;
using FoodieGo_Backend.Model;


namespace FoodieGo_Backend.Data
{
    public class ApplicationDbContext : DbContext

    {
        // Main DbContext for FoodieGo
        // Handles database tables and connections
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
