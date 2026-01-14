using Microsoft.AspNetCore.Mvc;
using StationeryMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace StationeryMVC.Controllers
{
    public class StationeryController : Controller
    {
        private static List<StationeryItem> items = new()
        {
            new StationeryItem { Id = 1, Name = "Pen", Category = "Writing", Quantity = 10, Price = 5 },
            new StationeryItem { Id = 2, Name = "Notebook", Category = "Books", Quantity = 5, Price = 25 }
        };

        private static readonly List<string> categories = new()
        {
            "Writing",
            "Books",
            "Office",
            "Art",
            "Other"
        };

        public IActionResult Index()
        {
            return View(items);
        }

        // GET
        public IActionResult Create()
        {
            ViewBag.Categories = categories;
            return View();
        }

        // POST
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
    }
}
