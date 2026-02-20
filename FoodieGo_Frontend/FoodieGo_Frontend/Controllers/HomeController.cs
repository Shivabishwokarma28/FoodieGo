using Microsoft.AspNetCore.Mvc;
using FoodieGo_Frontend.Models;
using System.Diagnostics;

namespace FoodieGo_Frontend.Controllers
{
    public class HomeController : Controller
    {
        // Static list acting as a temporary database for our food items
        private static readonly List<FoodItem> _allDishes = new List<FoodItem>
        {
            new FoodItem { Id = 1, Name = "Pepperoni Perfection", Price = 1250, Description = "Pizza: Spicy pepperoni and mozzarella.", ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=800" },
            new FoodItem { Id = 2, Name = "The Ultimate Burger", Price = 850, Description = "Burgers: Wagyu beef and aged cheddar.", ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=800" },
            new FoodItem { Id = 3, Name = "Matcha Iced Latte", Price = 450, Description = "Coffee: Ceremonial grade matcha.", ImageUrl = "https://images.unsplash.com/photo-1515823064-d6e0c04616a7?w=800" },
            new FoodItem { Id = 4, Name = "Berry Bliss Bowl", Price = 600, Description = "Desserts: Fresh organic acai and berries.", ImageUrl = "https://images.unsplash.com/photo-1590301157890-4810ed352733?w=800" },
            new FoodItem { Id = 5, Name = "Salmon Nigiri", Price = 1850, Description = "Sushi: Fresh Atlantic salmon.", ImageUrl = "https://images.unsplash.com/photo-1579871494447-9811cf80d66c?w=800" },
            new FoodItem { Id = 6, Name = "Truffle Penne", Price = 1100, Description = "Pasta: Creamy sauce with mushrooms.", ImageUrl = "https://images.unsplash.com/photo-1473093226795-af9932fe5856?w=800" },
            new FoodItem { Id = 7, Name = "Quinoa Harvest", Price = 750, Description = "Salads: Mixed greens and lemon tahini.", ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=800" },
            new FoodItem { Id = 8, Name = "Street Carnitas", Price = 950, Description = "Tacos: Slow-cooked pork.", ImageUrl = "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?w=800" }
        };

        // GET: Index - Displays the main dashboard with all available dishes
        public IActionResult Index()
        {
            // Passes the entire list of dishes to the Index view
            return View(_allDishes);
        }

        // GET: CategoryDetails - Filters dishes based on a category name (e.g., "Pizza")
        public IActionResult CategoryDetails(string name)
        {
            // Uses LINQ to find items where the description contains the category name
            var filtered = _allDishes
                .Where(x => x.Description.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Store the category name in ViewBag to display it as a heading in the View
            ViewBag.CategoryName = name;

            // Returns the CategoryDetails view with the filtered list
            return View(filtered);
        }

        // GET: Details - Displays full information for a single specific dish
        public IActionResult Details(int id)
        {
            // Find the first item matching the ID provided in the URL
            var item = _allDishes.FirstOrDefault(x => x.Id == id);

            // If no item is found, return a standard 404 error page
            if (item == null) return NotFound();

            return View(item);
        }

        // GET: Search - Allows users to find dishes by keyword in Name or Description
        public IActionResult Search(string query)
        {
            // If the search box is empty, just reload the full Index page
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            // Filter the list for any matches (case-insensitive)
            var results = _allDishes
                .Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            x.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Inform the user what they searched for via ViewBag
            ViewBag.SelectedCategory = $"Search results for '{query}'";

            // Reuse the "Index" view to display the search results instead of creating a new view
            return View("Index", results);
        }

        // GET: About - Returns the static 'About Us' information page
        public IActionResult About()
        {
            return View();
        }
    }
}