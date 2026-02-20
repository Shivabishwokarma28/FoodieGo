using FoodieGo_Backend.Data;
using FoodieGo_Backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FoodieGo_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RestaurantController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: api/restaurant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetAll()
        {
            return await context.Restaurants
                                .Include(r => r.FoodItems) // include menu items
                                .ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetById(int id)
        {
            var resturant = await context.Restaurants.Include(r => r.FoodItems).FirstOrDefaultAsync(r => r.Id == id);
            if (resturant == null)
            {
                return NotFound();
            }
            return resturant;
        }

        // POST: create a restaurant (anyone can add now)
        [HttpPost]
        public async Task<ActionResult<Restaurant>> Create(Restaurant restaurant)
        {
            await context.Restaurants.AddAsync(restaurant);
            await context.SaveChangesAsync();
            return Ok(restaurant);
        }
        //PUT: Updata a restaurant
        [HttpPut("{id}")]
        public async Task<ActionResult<Restaurant>> Update(int id, Restaurant update)
        {
            var restaurant = await context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            // Tell EF this object is modified
            context.Entry(update).State = EntityState.Modified;
            //Save change
            await context.SaveChangesAsync();
            return Ok(update);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Restaurant>> Delete(int id)
        {
            var restaurant = await context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            context.Restaurants.Remove(restaurant);
            await context.SaveChangesAsync();
            return Ok("Restaurant delete successfully");
        }
    }
}
