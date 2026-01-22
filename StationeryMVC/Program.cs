using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using StationeryMVC.Data;
using StationeryMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// ====================
// SERVICES
// ====================

// Add MVC
builder.Services.AddControllersWithViews();

// ✅ EF Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ✅ ASP.NET Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ✅ Session (for STEP 4: store user login info)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ====================
// APPLY MIGRATIONS & SEED DATA
// ====================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    // Seed AppSettings
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
// SEED ROLES & ADMIN USER
// ====================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // Create Admin role
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    // Create default admin user
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

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// ====================
// ROTATIVA CONFIG
// ====================
RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// ====================
// MIDDLEWARE
// ====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Session MUST come before Authentication
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// ====================
// ROUTING
// ====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Stationery}/{action=Index}/{id?}"
);

app.Run();
