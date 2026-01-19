using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using StationeryMVC.Data;
using StationeryMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// ====================
// SERVICES
// ====================
builder.Services.AddControllersWithViews();

// ✅ ApplicationDbContext (SINGLE DB)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ✅ STEP 2.3 (PART A): ADD IDENTITY WITH ROLES
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ====================
// BUILD THE APP
// ====================
var app = builder.Build();

// ====================
// APPLY MIGRATIONS & SEED SHOP SETTINGS
// ====================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apply migrations
    context.Database.Migrate();

    // Seed default shop settings
    if (!context.AppSettings.Any())
    {
        context.AppSettings.Add(new AppSettings
        {
            ShopName = "Triple L Stationery Shop",
            Slogan = "Everything You Need for School & Office",
            LogoPath = "/images/TripleL.png"
        });

        context.SaveChanges();
    }
}

// ====================
// ROTATIVA CONFIGURATION
// ====================
RotativaConfiguration.Setup(
    app.Environment.WebRootPath,
    "Rotativa"
);

// ====================
// MIDDLEWARE
// ====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // 1️⃣ Create Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // 2️⃣ Create default admin user
    string adminEmail = "admin@triplel.com";
    string adminPassword = "Lerato@91";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(adminUser, adminPassword);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ REQUIRED FOR LOGIN / ROLES
app.UseAuthentication();
app.UseAuthorization();

// ====================
// ROUTES
// ====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Stationery}/{action=Index}/{id?}"
);

app.Run();
