using Application.Repository;
using Application.Service;
using Infrastructure.Repository.PostgresDbContext;
using Infrastructure.Repository.Repository;
using Infrastructure.Service.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(o =>
    {
        o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
    });

builder.Services.AddScoped<IInventoryRepository,InventoryRepository>();
builder.Services.AddScoped<IItemRepository,ItemRepository>();
builder.Services.AddScoped<IItemValueRepository,ItemValueRepository>();
builder.Services.AddScoped<IInventoryTypeRepository, InventoryTypeRepository>();
builder.Services.AddScoped<IService,Service>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("Sleep");
    Thread.Sleep(5000);
    Console.WriteLine("Wake up");

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
