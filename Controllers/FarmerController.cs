using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;

namespace Web.Controllers
{
    // Handles product management and profile creation for logged-in farmers
    // @see https://learn.microsoft.com/en-us/ef/core/querying/related-data
    // @see https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions

    public class FarmerController : Controller
    {
        private readonly AppDbContext _db;

        public FarmerController(AppDbContext db)
        {
            _db = db;
        }

        // Shows all products belonging to the logged-in farmer
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null || HttpContext.Session.GetString("Role") != "Farmer")
                return RedirectToAction("Login", "Auth");

            var farmer = await _db.Farmers
                                  .Include(f => f.Products)
                                  .FirstOrDefaultAsync(f => f.Name == username);

            if (farmer == null)
                return RedirectToAction("CreateProfile");

            return View(farmer.Products.ToList());
        }

        // GET: Shows the form for adding a new product
        [HttpGet]
        public IActionResult AddProduct()
        {
            if (HttpContext.Session.GetString("Role") != "Farmer")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // POST: Saves a new product to the logged-in farmer's profile
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null || HttpContext.Session.GetString("Role") != "Farmer")
                return RedirectToAction("Login", "Auth");

            var farmer = await _db.Farmers.FirstOrDefaultAsync(f => f.Name == username);
            if (farmer == null)
                return RedirectToAction("CreateProfile");

            model.FarmerId = farmer.Id;
            _db.Products.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Show form to create a farmer profile if one doesn't exist
        [HttpGet]
        public IActionResult CreateProfile()
        {
            return View();
        }

        // POST: Create the farmer profile using session's username
        [HttpPost]
        public async Task<IActionResult> CreateProfile(string location)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
                return RedirectToAction("Login", "Auth");

            var farmer = new Farmer
            {
                Name = username,
                Location = location
            };

            _db.Farmers.Add(farmer);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
