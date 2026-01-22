using System.Collections.Generic;
using StationeryMVC.Models; // Needed for StationeryItem

namespace StationeryMVC.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public int TotalCategories { get; set; }

        // 🔹 Add low-stock items for dashboard
        public List<StationeryItem> LowStockItems { get; set; } = new();
    }
}
