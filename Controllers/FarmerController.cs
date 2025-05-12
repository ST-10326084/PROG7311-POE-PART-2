using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;

namespace Web.Controllers
{
    public class FarmerController : Controller
    {
        private readonly AppDbContext _db;

        public FarmerController(AppDbContext db)
        {
            _db = db;
        }

        // Show products that belong to the logged-in farmer
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

        // GET: Show Add Product Form
        [HttpGet]
        public IActionResult AddProduct()
        {
            if (HttpContext.Session.GetString("Role") != "Farmer")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // POST: Handle Add Product
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

        // If Farmer doesn't exist, show create profile form
        [HttpGet]
        public IActionResult CreateProfile()
        {
            return View();
        }

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
