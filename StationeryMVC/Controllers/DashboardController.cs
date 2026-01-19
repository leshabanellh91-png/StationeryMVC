using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StationeryMVC.Data;
using System.Globalization;
using System.Linq;

[Authorize(Roles = "Admin")] // 🔒 ADMIN ONLY
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var items = _context.StationeryItems.ToList();

        ViewBag.TotalItems = items.Count;
        ViewBag.TotalQuantity = items.Sum(i => i.Quantity);
        ViewBag.TotalValue = items.Sum(i => i.Price * i.Quantity);
        ViewBag.TotalCategories = items
            .Select(i => i.Category)
            .Distinct()
            .Count();

        return View();
    }
}
