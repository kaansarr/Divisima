using Divisima.BL.Repositories;
using Divisima.DAL.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews().AddJsonOptions(x =>
//                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddScoped(typeof(IRepository<>),typeof(SqlRepository<>));
//AddSingleton: Bir request iþleminde nesneden uygulama yaþamý boyunca 1 tane üretilir
//AddScoped: Ayný request iþleminde nesneden 1 tane üretilir.
//AddTransient: //Her request iþleminde yenibir nesne üretilir.
builder.Services.AddDbContext<SqlContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CS1")));
//custom Authentication in .Net Core, (Asp.Net Memberhip)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt=>
{ 
    opt.ExpireTimeSpan=TimeSpan.FromMinutes(60);
    opt.LoginPath= "/admin/login";
    opt.LogoutPath = "/admin/logout";
});

var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseStatusCodePagesWithRedirects("/hata/{0}");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();//Kimlik Doðrulama
app.UseAuthorization();//Kimlik Yetkilendirme
app.MapControllerRoute(name: "admin", pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");
app.MapControllerRoute(name:"default",pattern:"{controller=home}/{action=index}/{id?}");
//app.UseMvcWithDefaultRoute();

app.Run();
