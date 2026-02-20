using System.ComponentModel.DataAnnotations;

namespace FoodieGo_Frontend.Models
{
    public class Register
    {

        // User's full name
        // [Required] ensures client must provide this field
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        // User's email address
        // [Required] ensures the field is not empty
        // [RegularExpression] validates email format
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
         ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        // User's password
        // Validates password strength using regex:
        // - At least one lowercase letter
        // - At least one uppercase letter
        // - At least one number
        // - At least one special character
        // - Minimum length 8 characters
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be 8+ chars, include upper, lower, number & symbol")]
        public string Password { get; set; }
    }
}
