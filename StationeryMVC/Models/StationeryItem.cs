using System.ComponentModel.DataAnnotations;

namespace StationeryMVC.Models
{
    public class StationeryItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 1000)]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(1.00, 10000.00, ErrorMessage = "Price must be between R1,00 and R10 000,00")]
        public decimal Price { get; set; }


        // Stores relative path of uploaded image
        [Display(Name = "Image")]
        public string ImageUrl { get; set; } = "/images/default.png";
    }
}
