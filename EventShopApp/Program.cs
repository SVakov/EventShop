using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Services;
using EventShopApp.Services.Authorization;
using EventShopApp.Services.Implementation;
using EventShopApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true; 
    options.Password.RequireLowercase = true; 
    options.Password.RequireUppercase = true; 
    options.Password.RequireNonAlphanumeric = true; 
    options.Password.RequiredLength = 8; 
    options.Password.RequiredUniqueChars = 1;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFlowerService, FlowerService>();
builder.Services.AddScoped<IArrangementService, ArrangementService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddHostedService<CartCleanupService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagementAccess", policy =>
        policy.RequireAssertion(context =>
        {
            var userEmail = context.User.Identity?.Name;
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var employee = dbContext.Employees.FirstOrDefault(e => e.Email == userEmail);
            return employee != null &&
                   (employee.Role == EmployeeRole.Owner);
        }));
});

builder.Services.AddScoped<IAuthorizationHandler, ManagementAccessHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
