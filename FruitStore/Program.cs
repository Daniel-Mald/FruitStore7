using FruitStore.Models.Entities;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<Repository<Categorias>>();
builder.Services.AddTransient<Repository<Usuarios>>();
builder.Services.AddTransient<ProductosRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.AccessDeniedPath = "/Home/Denied";
    x.LoginPath = "/Home/Login";
    x.LogoutPath = "/Home/Logout";
    x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    x.Cookie.Name = "FruteriaCookie";
});

builder.Services.AddDbContext<FruteriashopContext>(x => x.UseMySql("server=localhost;user=root;password=root;database=fruteriashop",
	Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql")));

builder.Services.AddMvc();
var app = builder.Build();

app.UseFileServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapDefaultControllerRoute();

app.Run();
