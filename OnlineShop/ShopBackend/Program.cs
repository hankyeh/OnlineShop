using Microsoft.EntityFrameworkCore;
using ShopBackend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopBackEndContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopBackEndContext")));

//©I¥sAPI ¨Ï¥Î
builder.Services.AddHttpClient();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
