using System.ComponentModel.DataAnnotations;

namespace FoodieGo_Backend.DTOs
{
    // Data Transfer Object (DTO) for user login
    // Defines the data client must send to authenticate
    public class LoginDto
    {
        // User's email address
        // [Required] ensures the field is not empty
        // [RegularExpression] validates proper email format
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        // User's password
        // [Required] ensures password is provided
        // [RegularExpression] validates password strength:
        // - At least 1 lowercase, 1 uppercase, 1 number, 1 special character
        // - Minimum length 8 characters
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be 8+ chars, include upper, lower, number & symbol")]
        public string Password { get; set; }
    }
}
