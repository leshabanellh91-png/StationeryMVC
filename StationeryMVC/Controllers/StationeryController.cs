using Microsoft.AspNetCore.Mvc;
using StationeryMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace StationeryMVC.Controllers
{
    public class StationeryController : Controller
    {
        // In-memory stationery items
        private static List<StationeryItem> items = new()
        {
            new StationeryItem { Id = 1, Name = "Pen", Category = "Writing", Quantity = 10, Price = 5 },
            new StationeryItem { Id = 2, Name = "Notebook", Category = "Books", Quantity = 5, Price = 25 }
        };

        // Categories
        private static readonly List<string> categories = new()
        {
            "Writing",
            "Books",
            "Office",
            "Art",
            "Other"
        };

        // INDEX - List with Search
        public IActionResult Index(string searchString, string category)
        {
            var filteredItems = items.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                filteredItems = filteredItems.Where(i => i.Name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(category))
                filteredItems = filteredItems.Where(i => i.Category == category);

            ViewBag.Categories = categories;
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = category;

            return View(filteredItems.ToList());
        }

        // GET CREATE
        public IActionResult Create()
        {
            ViewBag.Categories = categories;
            return View();
        }

        // POST CREATE
        [HttpPost]
        public IActionResult Create(StationeryItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = categories;
                return View(item);
            }

            item.Id = items.Any() ? items.Max(i => i.Id) + 1 : 1;
            items.Add(item);
            return RedirectToAction(nameof(Index));
        }

        // GET EDIT
        public IActionResult Edit(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();

            ViewBag.Categories = categories;
            return View(item);
        }

        // POST EDIT
        [HttpPost]
        public IActionResult Edit(StationeryItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = categories;
                return View(item);
            }

            var existing = items.First(i => i.Id == item.Id);
            existing.Name = item.Name;
            existing.Category = item.Category;
            existing.Quantity = item.Quantity;
            existing.Price = item.Price;

            return RedirectToAction(nameof(Index));
        }

        // GET DELETE - confirmation
        public IActionResult Delete(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST DELETE
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item != null)
                items.Remove(item);

            return RedirectToAction(nameof(Index));
        }

        // DASHBOARD
        public IActionResult Dashboard()
        {
            ViewBag.TotalItems = items.Count;
            ViewBag.TotalStock = items.Sum(i => i.Quantity);
            ViewBag.CategoryCount = items.GroupBy(i => i.Category).Count();

            return View();
        }
    }
}
