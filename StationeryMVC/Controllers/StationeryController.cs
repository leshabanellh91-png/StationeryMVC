using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StationeryMVC.Data;
using StationeryMVC.Models;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace StationeryMVC.Controllers
{
    public class StationeryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StationeryController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Show Create Form
        public IActionResult Create()
        {
            PopulateCategories();
            return View();
        }

        // POST: Handle Form Submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StationeryItem item, IFormFile imageFile)
        {
            PopulateCategories(); // Repopulate for validation errors

            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    item.ImageUrl = "/images/" + uniqueFileName;
                }
                else
                {
                    item.ImageUrl = "/images/default.png"; // default image
                }

                _context.StationeryItems.Add(item);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        // GET: Dashboard
        public IActionResult Dashboard()
        {
            // Fetch all stationery items (never null)
            var items = _context.StationeryItems.ToList() ?? new List<StationeryItem>();

            // Calculate totals for dashboard
            ViewBag.TotalItems = items.Count;
            ViewBag.TotalQuantity = items.Sum(i => i.Quantity);
            ViewBag.TotalCategories = items.Select(i => i.Category).Distinct().Count();
            ViewBag.TotalValue = items.Sum(i => i.Price * i.Quantity);

            // Ensure categories exist for dropdowns
            PopulateCategories();

            // Use the full path to the existing Index.cshtml view
            return View("~/Views/Stationery/Index.cshtml", items);
        }

        // GET: List all items
        public IActionResult Index()
        {
            // Fetch all stationery items from the database (never null)
            var items = _context.StationeryItems.ToList() ?? new List<StationeryItem>();

            // Ensure categories exist for the view
            PopulateCategories();

            // Calculate totals for dashboard section
            ViewBag.TotalItems = items.Count;
            ViewBag.TotalQuantity = items.Sum(i => i.Quantity);
            ViewBag.TotalCategories = items.Select(i => i.Category).Distinct().Count();
            ViewBag.TotalValue = items.Sum(i => i.Price * i.Quantity);

            // Return items to the view
            return View(items);
        }

        // Helper method to populate categories
        private void PopulateCategories()
        {
            var categories = new List<string>
            {
                "Books",
                "Writing",
                "Art Supplies",
                "Office Supplies",
                "Paper Products",
                "Organizational Tools"
            };

            ViewBag.Categories = new SelectList(categories);
        }
    }
}
