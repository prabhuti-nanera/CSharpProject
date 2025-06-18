using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EventManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add MVC and Razor Pages services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Added for Razor Pages support

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed roles and users
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string[] roleNames = { "Admin", "Organizer", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
    var adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com" };
    var organizerUser = new IdentityUser { UserName = "organizer@example.com", Email = "organizer@example.com" };
    string adminPassword = "Admin@123";
    string organizerPassword = "Organizer@123";
    if (await userManager.FindByEmailAsync(adminUser.Email) == null)
    {
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    if (await userManager.FindByEmailAsync(organizerUser.Email) == null)
    {
        var result = await userManager.CreateAsync(organizerUser, organizerPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(organizerUser, "Organizer");
        }
    }
}

app.Run();