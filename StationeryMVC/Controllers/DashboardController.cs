using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StationeryMVC.Data;
using StationeryMVC.Models.ViewModels;
using System.Linq;

namespace StationeryMVC.Controllers
{
    [Authorize(Roles = "Admin")] // 🔒 Only admins
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get all stationery items
            var items = _context.StationeryItems.ToList();

            // Prepare the dashboard model
            var model = new DashboardViewModel
            {
                TotalItems = items.Count,
                TotalQuantity = items.Sum(i => i.Quantity),
                TotalValue = items.Sum(i => i.Price * i.Quantity),
                TotalCategories = items.Select(i => i.Category).Distinct().Count(),
                LowStockItems = items.Where(i => i.Quantity <= 10).ToList()
            };

            return View(model);
        }
    }
}
