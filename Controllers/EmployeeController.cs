using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;

namespace Web.Controllers
{
    // Manages all Employee-related actions (viewing farmers, filtering products)
    // @see https://learn.microsoft.com/en-us/ef/core/querying/related-data
    // @see https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

        // Returns true if current session role is Employee
        private bool IsEmployee()
        {
            return HttpContext.Session.GetString("Role") == "Employee";
        }

        // Dashboard entry point for employees
        public IActionResult Index()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // Shows list of all farmers and their product count
        public async Task<IActionResult> Farmers()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");

            var farmers = await _db.Farmers
                                   .Include(f => f.Products)
                                   .ToListAsync();

            return View(farmers);
        }

        // GET: Show form to add new farmer
        [HttpGet]
        public IActionResult AddFarmer()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // POST: Add a new farmer to the system
        [HttpPost]
        public async Task<IActionResult> AddFarmer(string name, string location)
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");

            var farmer = new Farmer { Name = name, Location = location };
            _db.Farmers.Add(farmer);
            await _db.SaveChangesAsync();

            return RedirectToAction("Farmers");
        }

        // Shows list of all products with optional filtering
        [HttpGet]
        public async Task<IActionResult> Products(string category, DateTime? fromDate, DateTime? toDate)
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Auth");

            var products = _db.Products
                              .Include(p => p.Farmer)
                              .AsQueryable();

            // Apply category filter
            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category == category);

            // Apply date range filters
            if (fromDate.HasValue)
                products = products.Where(p => p.ProductionDate >= fromDate.Value);

            if (toDate.HasValue)
                products = products.Where(p => p.ProductionDate <= toDate.Value);

            return View(await products.ToListAsync());
        }
    }
}
