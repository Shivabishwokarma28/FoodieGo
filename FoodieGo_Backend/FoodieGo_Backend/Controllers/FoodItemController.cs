using FoodieGo_Backend.Data;
using FoodieGo_Backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodieGoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public FoodItemController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET ALL FOOD ITEMS

        // This method returns all food items
        // It also includes the Restaurant information
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetAll()
        {
            return context.FoodItems.Include(f => f.Restaurant).ToList();
        }
        // GET FOOD ITEM BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetById(int id)
        {
            // Find food item including restaurant
            var item = await context.FoodItems.Include(f => f.Restaurant).FirstOrDefaultAsync(f => f.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            // Return found item
            return item;
        }
        // CREATE FOOD ITEM
        // This adds a new food item to database
        [HttpPost]
        public async Task<ActionResult<FoodItem>> Create(FoodItem item)
        {
            //Add new food  to database
            await context.FoodItems.AddAsync(item);
            // Save Change to database
            await context.SaveChangesAsync();
            return Ok(item);
        }
        // UPDATE FOOD ITEM (FULL UPDATE)
        [HttpPut("{id}")]
        public async Task<ActionResult<FoodItem>> Update(int id, FoodItem update)
        {
            // Check if URL id matches body id
            if (id != update.Id)
            {
                return BadRequest("Id Mismatch");
            }
            // Check if URL id matches body id
            context.Entry(update).State = EntityState.Modified;
            //SaveChange database
            await context.SaveChangesAsync();
            return Ok(update);
        }
        // DELETE FOOD ITEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Find food item by id
            var item = await context.FoodItems.FindAsync(id);

            // If not found return 404
            if (item == null)
                return NotFound();

            // Remove from database
            context.FoodItems.Remove(item);

            // Save changes
            await context.SaveChangesAsync();

            return Ok("Food item deleted successfully");
        }

    }
}
