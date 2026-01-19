using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StationeryMVC.Models
{
    public class QuotationItem
    {
        public int Id { get; set; }

        // -------------------------
        // Quotation relationship
        // -------------------------
        public int QuotationId { get; set; }
        public Quotation Quotation { get; set; }

        // -------------------------
        // Stationery relationship
        // -------------------------
        // ✅ Fixes StationeryItemId errors
        public int StationeryItemId { get; set; }

        // ✅ Fixes StationeryItem navigation errors
        public StationeryItem StationeryItem { get; set; }

        // -------------------------
        // Pricing
        // -------------------------
        // ✅ Fixes Price errors
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }


        public int Quantity { get; set; }

        // Computed (not stored)
        [NotMapped]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

    }
}
