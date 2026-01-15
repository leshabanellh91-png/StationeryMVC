using Microsoft.EntityFrameworkCore;
using StationeryMVC.Models;
using System.Collections.Generic;

namespace StationeryMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // This tells EF Core to create a table for StationeryItem
        public DbSet<StationeryItem> StationeryItems { get; set; }
    

public DbSet<Quotation> Quotations { get; set; }
public DbSet<QuotationItem> QuotationItems { get; set; }

    }
}