using System.ComponentModel.DataAnnotations;

namespace StationeryMVC.Models
{
    public class StationeryItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
