using Microsoft.AspNetCore.Mvc;
using StationeryMVC.Data;
using StationeryMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace StationeryMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================
        // GET: Login
        // ====================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ====================
        // POST: Login
        // ====================
        [HttpPost]
        public IActionResult Login(string Email, string Password, bool RememberMe)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ViewBag.Error = "Email and password are required";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            // Store session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToAction("Index", "Home");
        }

        // ====================
        // GET: Register
        // ====================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ====================
        // POST: Register
        // ====================
        [HttpPost]
        public IActionResult Register(string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View();
            }

            if (_context.Users.Any(u => u.Email == Email))
            {
                ViewBag.Error = "Email already exists";
                return View();
            }

            var user = new User
            {
                Email = Email,
                Password = Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ====================
        // Logout
        // ====================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
