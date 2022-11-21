using FiorEllo.Helpers;
using FiorEllo.Models;
using front_to_back.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

})
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();
app.MapControllerRoute(
    name:"areas",
     pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
app.MapDefaultControllerRoute();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

var scopFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using(var scope = scopFactory.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    var roleManager=scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
   await DbInitializer.SeedAsync(roleManager, userManager);
}

app.Run();
