using Excursiones.Data.Db;
using Excursiones.Data.Inyecciones;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDataAccess(builder.Configuration.GetConnectionString("ExcursionesDb"));

builder.Services.AddDbContext<ExcursionesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ExcursionesDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
