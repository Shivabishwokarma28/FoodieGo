using FoodieGo_Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace FoodieGo_Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        // Constructor: Injecting IHttpClientFactory to manage API connections efficiently
        public AuthController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

 
        /// Displays the Registration Page
  
        [HttpGet]
        public IActionResult Register() => View();

        /// Handles the Registration Form Submission

        [HttpPost]
        public async Task<IActionResult> Register(Register reg)
        {
            // Client-side & Model validation check
            if (!ModelState.IsValid)
            {
                return View(reg);
            }

            // Create client using the name defined in Program.cs
            var client = clientFactory.CreateClient("FoodieGo_Backend");

            // Send data to Backend API
            var response = await client.PostAsJsonAsync("Auth/Register", reg);

            if (!response.IsSuccessStatusCode)
            {
                // Capture API-specific errors (e.g., duplicate email) and display them to user
                ModelState.AddModelError("", "Registration failed. Email might already be in use.");
                return View(reg);
            }

            // On success, move user to Login page
            return RedirectToAction("Login");
        }

        /// Displays the Login Page
 
        [HttpGet]
        public IActionResult Login() => View();

  
        /// Handles Login Logic and Session Management
   
        [HttpPost]
        public async Task<IActionResult> LogIn(Login log)
        {
            if (!ModelState.IsValid) return View(log);

            var client = clientFactory.CreateClient("FoodieGo_Backend");
            var response = await client.PostAsJsonAsync("Auth/Login", log);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View(log);
            }

            // Map the JSON response from API to our ViewModel
            var result = await response.Content.ReadFromJsonAsync<LoginResponseViewModel>();

            if (result != null)
            {
                // 1. Store the JWT Token in Session for authorized API calls later
                HttpContext.Session.SetString("JWToken", result.Token);

                // 2. Store Display Name. 
                // Priority: 1. API Returned Name -> 2. User's Login Email -> 3. "Guest" fallback
                // This solves the 'ArgumentNullException' by ensuring value is never null.
                HttpContext.Session.SetString("UserName", result.Name ?? log.Email ?? "Guest");
            }

            return RedirectToAction("Index", "Home");
        }


        /// Logs the user out by clearing the session

        [HttpGet] // Changed to Get for easy access via Nav links
        public IActionResult Logout()
        {
            // Completely wipes all session keys (Token, UserName, etc.)
            HttpContext.Session.Clear();

            // Return to Home Page as a public user
            return RedirectToAction("Index", "Home");
        }
    }
}