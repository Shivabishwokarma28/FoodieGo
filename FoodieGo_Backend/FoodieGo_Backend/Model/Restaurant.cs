using System.ComponentModel.DataAnnotations;
using FoodieGo_Backend.Model;

namespace FoodieGo_Backend.Model
{
    //Represent a restaurant

    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        // Navigation property: one restaurant has many food items
        public ICollection<FoodItem> FoodItems { get; set; }
    }
}
