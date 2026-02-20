using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodieGo_Backend.Model;

namespace FoodieGo_Backend.Model
{
    // Represents a food item in a restaurant menu

    public class FoodItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Foreign key to the restaurant
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
