using FoodieGo_Backend.Model;
using System.ComponentModel.DataAnnotations;

namespace FoodieGo_Backend.Model
{
    public class Order
    {
        // Represents an order placed by a user
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        // For simplicity, list of food items (could be another table for order details)
        // Navigation property: a collection of FoodItems that belong to this Restaurant
        // - Represents a "one-to-many" relationship (one restaurant has many food items)
        // - EF Core can use this to load all related food items for a restaurant

        // Navigation property: list of ordered items (with quantity)
        public ICollection<FoodItem> FoodItems { get; set; }


        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
