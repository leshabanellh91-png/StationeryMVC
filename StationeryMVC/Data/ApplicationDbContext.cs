using Microsoft.EntityFrameworkCore;
using StationeryMVC.Models;

namespace StationeryMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StationeryItem> StationeryItems { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationItem> QuotationItems { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
    }
}
