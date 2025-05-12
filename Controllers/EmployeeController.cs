using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;

namespace Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

        private bool IsEmployee()
        {
            return HttpContext.Session.GetString("Role") == "Employee";
        }

        public IActionResult Index()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // View all farmers
        public async Task<IActionResult> Farmers()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");

            var farmers = await _db.Farmers.Include(f => f.Products).ToListAsync();
            return View(farmers);
        }

        // Add new farmer (GET)
        [HttpGet]
        public IActionResult AddFarmer()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // Add new farmer (POST)
        [HttpPost]
        public async Task<IActionResult> AddFarmer(string name, string location)
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");

            var farmer = new Farmer { Name = name, Location = location };
            _db.Farmers.Add(farmer);
            await _db.SaveChangesAsync();

            return RedirectToAction("Farmers");
        }

        // View all products with filters
        [HttpGet]
        public async Task<IActionResult> Products(string category, DateTime? fromDate, DateTime? toDate)
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");

            var products = _db.Products.Include(p => p.Farmer).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category == category);

            if (fromDate.HasValue)
                products = products.Where(p => p.ProductionDate >= fromDate.Value);

            if (toDate.HasValue)
                products = products.Where(p => p.ProductionDate <= toDate.Value);

            return View(await products.ToListAsync());
        }
    }
}
