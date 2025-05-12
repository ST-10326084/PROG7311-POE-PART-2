using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Core.Models;

namespace Web.Controllers
{
    // Handles user login, registration, and logout actions via AuthService
    // @see https://learn.microsoft.com/en-us/dotnet/api/system.web.mvc.controller
    // @see https://learn.microsoft.com/en-us/aspnet/core/security/authentication/
    // @see https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpsession

    public class AuthController : Controller
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        // Displays login form
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Handles login submission and session setup
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _auth.LoginAsync(username, password);
            if (token == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            // Session stores the logged-in username and role
            var user = await _auth.GetUserByUsernameAsync(username);
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", user.Role == "Farmer" ? "Farmer" : "Employee");
        }

        // Displays registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handles registration form submission
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

        // Clears session and redirects to login
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
