using Application.Repository;
using Application.Service;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Infrastructure.Repository.Repository;
using Infrastructure.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Application.Mapping.MappingProfile));

builder.Services.AddDbContext<AppDbContext>(o =>
    {
        o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
    });

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IInventoryRepository,InventoryRepository>();
builder.Services.AddScoped<IItemRepository,ItemRepository>();
builder.Services.AddScoped<IItemValueRepository,ItemValueRepository>();
builder.Services.AddScoped<IInventoryTypeRepository, InventoryTypeRepository>();
builder.Services.AddScoped<IService,Service>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

app.UseStaticFiles();
using (var scope = app.Services.CreateScope())
{
    Thread.Sleep(5000);
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

using (var scope = app.Services.CreateScope())
{
    var nameAdmin = "werty";
    var passAdmin = "1111";
    string[] roleNames = { "Admin", "Registered" };

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    if (await userManager.FindByNameAsync(nameAdmin) == null)
    {
        var admin = new AppUser { UserName = nameAdmin, Email = nameAdmin };
        await userManager.CreateAsync(admin, passAdmin);
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();