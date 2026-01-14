using System.ComponentModel.DataAnnotations;

namespace StationeryMVC.Models
{
    public class StationeryItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 1000)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;

        
    }
}
