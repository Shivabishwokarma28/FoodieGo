using FoodieGo_Backend.Data;
using FoodieGo_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodieGo_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public OrderController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET ALL ORDERS
        // Admin only: view all orders

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            // Include FoodItems and User for each order
            return await context.Orders
                                 .Include(o => o.FoodItems)
                                 .Include(o => o.User)
                                 .ToListAsync();
        }
        // GET ORDERS FOR LOGGED-IN USER
        [HttpGet("Myorders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
        {
            // Get logged-in user's email from JWT claims
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return Unauthorized();

            // Get all orders for this user
            var orders = await context.Orders
                                       .Where(o => o.UserId == user.Id)
                                       .Include(o => o.FoodItems)
                                       .ToListAsync();

            return orders;
        }



        // GET ORDER BY ID (User or Admin)

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            // Find order including food items and user
            var order = await context.Orders
                                      .Include(o => o.FoodItems)
                                      .Include(o => o.User)
                                      .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound(); // 404 if order does not exist

            return order;
        }


        // CREATE A NEW ORDER

        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order order)
        {
            // Optional: calculate total amount
            if (order.FoodItems != null && order.FoodItems.Count > 0)
            {
                order.TotalAmount = order.FoodItems.Sum(f => f.Price);
            }

            // Add order to database
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            // Return created order with 200 OK
            return Ok(order);
        }


        // UPDATE ORDER STATUS
        //[FromBody] tells ASP.NET Core to read the value from the request body, not from the URL or query string.
        // Admin only

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.Status = status; // e.g., Pending, Completed, Cancelled
            await context.SaveChangesAsync();

            return Ok(order);
        }

    }
}
