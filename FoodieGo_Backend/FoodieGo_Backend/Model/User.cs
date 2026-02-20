using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FoodieGo_Backend.Model;

namespace FoodieGo_Backend.Model
{
    // Represents a user (customer) in the system
    public class User
    {
        [Key]
        public int Id { get; set; }  // Primary key

        [Required, MaxLength(50)]
        public string Name { get; set; } // User's full name

        [Required, MaxLength(100)]
        public string Email { get; set; } // User email for login

        [Required]
        public string PasswordHash { get; set; } // Hashed password (never plain text)

        public string Role { get; set; } = "Customer"; // default role

        // Navigation property: list of orders by this user
        public ICollection<Order> Orders { get; set; }
    }
}
