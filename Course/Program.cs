using Application.Repository.Tables;
using Application.Repository.User;
using Application.Service;
using Domain.Models;
using Elastic.Clients.Elasticsearch;
using Infrastructure.Elastic.EditorModel;
using Infrastructure.Elastic.ElasticSearch;
using Infrastructure.Repository.PostgresDbContext;
using Infrastructure.Repository.Repository.Tables;
using Infrastructure.Repository.Repository.User;
using Infrastructure.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
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
    options.AccessDeniedPath = "/User/AccessDenied";
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var settings = new ElasticsearchClientSettings(new Uri("http://elasticsearch:9200"))
    .DefaultIndex("editor_index");

var client = new ElasticsearchClient(settings);
builder.Services.AddSingleton(client);

builder.Services.AddScoped<IInventoryRepository,InventoryRepository>();
builder.Services.AddScoped<IItemRepository,ItemRepository>();
builder.Services.AddScoped<IItemValueRepository,ItemValueRepository>();
builder.Services.AddScoped<IInventoryTypeRepository, InventoryTypeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEditorRepository, EditorRepository>();
builder.Services.AddScoped<IEditorSearchService, EditorSearchService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IService,Service>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseStaticFiles();
using (var scope = app.Services.CreateScope())
{
    Thread.Sleep(30 * 1000);
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

using (var scope = app.Services.CreateScope())
{
    var nameAdmin = "werty";
    var emailAdmin = "werty@mail";
    var passAdmin = "1111";
    string[] roleNames = { "Admin", "Registered" };

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    var editorSearchService = scope.ServiceProvider.GetRequiredService<IEditorSearchService>();

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    if (await userManager.FindByEmailAsync(emailAdmin) == null)
    {
        var admin = new AppUser { Name = nameAdmin, UserName = emailAdmin, Email = emailAdmin };
        await userManager.CreateAsync(admin, passAdmin);
        await userManager.AddToRoleAsync(admin, "Admin");
        await userManager.AddToRoleAsync(admin, "Registered");
        await editorSearchService.IndexUserAsync(admin);
    }
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Register}/{id?}");

app.Run();