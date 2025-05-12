using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Core.Models;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _auth.LoginAsync(username, password);
            if (token == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            // Extract role from token (for simplicity in this demo)
            var user = await _auth.GetUserByUsernameAsync(username); // <-- Add this method in AuthService
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", user.Role == "Farmer" ? "Farmer" : "Employee");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string role)
        {
            var user = await _auth.RegisterAsync(username, password, role);
            if (user == null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
