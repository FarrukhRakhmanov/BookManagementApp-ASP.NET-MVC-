using Final_Project_Farrukh_Rakhmanov.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(Options => { Options.IdleTimeout = TimeSpan.FromSeconds(600); });
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserAccountContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookManagementContext")));
var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Enable session middleware

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();


