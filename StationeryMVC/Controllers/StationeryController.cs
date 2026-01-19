using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;


using StationeryMVC.Data;
using StationeryMVC.Models;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace StationeryMVC.Controllers
{
    public class StationeryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<StationeryController> _logger;

        public StationeryController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            ILogger<StationeryController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // ===============================
        // INDEX – Stationery List
        // ===============================
        public IActionResult Index()
        {
            var dbName = _context.Database.GetDbConnection().Database;
            _logger.LogInformation("EF Core is connected to database: {Database}", dbName);

            var items = _context.StationeryItems.ToList();
            return View(items);
        }

        // ===============================
        // DASHBOARD – Summary Only
        // ===============================
        public IActionResult Dashboard()
        {
            var items = _context.StationeryItems.ToList();

            ViewBag.TotalItems = items.Count;
            ViewBag.TotalQuantity = items.Sum(i => i.Quantity);

            ViewBag.TotalCategories = items
                .Where(i => !string.IsNullOrWhiteSpace(i.Category))
                .Select(i => i.Category)
                .Distinct()
                .Count();

            ViewBag.TotalValue = items.Sum(i => i.Price * i.Quantity);

            return View();
        }

        // ===============================
        // CREATE – GET
        // ===============================
        public IActionResult Create()
        {
            PopulateCategories();
            return View();
        }

        // ===============================
        // CREATE – POST
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StationeryItem item, IFormFile imageFile)
        {
            PopulateCategories();

            if (!ModelState.IsValid)
                return View(item);

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only JPG, PNG, or GIF images are allowed.");
                    return View(item);
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                try
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    imageFile.CopyTo(stream);

                    item.ImageUrl = "/images/" + fileName;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Image upload failed");
                    ModelState.AddModelError("", "Image upload failed.");
                    return View(item);
                }
            }
            else
            {
                item.ImageUrl = "/images/default.png";
            }

            try
            {
                _context.StationeryItems.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database save failed");
                ModelState.AddModelError("", "Failed to save item.");
                return View(item);
            }

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // EDIT – GET
        // ===============================
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var item = _context.StationeryItems.Find(id);
            if (item == null)
                return NotFound();

            PopulateCategories();
            return View(item);
        }
        [Authorize(Roles = "Admin")]
public IActionResult Edit(int id)
{
    var item = _context.StationeryItems.Find(id);

    if (item == null)
        return NotFound();

    PopulateCategories();
    return View(item);
}


        // ===============================
        // EDIT – POST
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, StationeryItem item, IFormFile imageFile)
        {
            if (id != item.Id)
                return NotFound();

            PopulateCategories();

            if (!ModelState.IsValid)
                return View(item);

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only JPG, PNG, or GIF images are allowed.");
                    return View(item);
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                try
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    imageFile.CopyTo(stream);
                    item.ImageUrl = "/images/" + fileName;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Image upload failed");
                    ModelState.AddModelError("", "Image upload failed.");
                    return View(item);
                }
            }
            else
            {
                // Preserve existing ImageUrl
                var existingItem = _context.StationeryItems.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (existingItem != null)
                    item.ImageUrl = existingItem.ImageUrl ?? "/images/default.png";
            }

            try
            {
                _context.Update(item);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.StationeryItems.Any(e => e.Id == item.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // DELETE – GET (confirmation)
        // ===============================
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var item = _context.StationeryItems.FirstOrDefault(m => m.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // ===============================
        // DELETE – POST
        // ===============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _context.StationeryItems.Find(id);
            if (item != null)
            {
                _context.StationeryItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index)); 
        }

        // ===============================
        // HELPERS
        // ===============================
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
