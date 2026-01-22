using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StationeryMVC.Models;

namespace StationeryMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // App settings
        public DbSet<AppSettings> AppSettings { get; set; }

        public DbSet<StationeryItem> StationeryItems { get; set; }

        // Quotations
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationItem> QuotationItems { get; set; }
         public DbSet<User> Users { get; set; }

        // Decimal precision fix
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Quotation>()
                .Property(q => q.TotalAmount)
                .HasPrecision(18, 2);
        }
    }
}
