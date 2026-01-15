using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StationeryMVC.Models
{
    public class Quotation
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public List<QuotationItem> Items { get; set; } = new List<QuotationItem>();
    }

    public class QuotationItem
    {
        public int Id { get; set; }

        public int QuotationId { get; set; }
        public Quotation Quotation { get; set; }

        public int StationeryItemId { get; set; }
        public StationeryItem StationeryItem { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
