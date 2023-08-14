using Divisima.BL.Repositories;
using Divisima.DAL.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<SqlContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CS1")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
builder.Services.AddCors(opt =>
{
    //opt.AddPolicy(name: "izinverilenler", policy => { policy.WithOrigins("http://localhost:5256").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); });

    //tüm originlere izin
    //opt.AddPolicy(name: "izinverilenler", policy => { policy.AllowAnyOrigin(); });
    //opt.AddPolicy(name: "izinverilenler", policy => { policy.SetIsOriginAllowed(x=>true); });

});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:5256",
        ValidAudience = "n11",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("benimözelkeybilgisi"))
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sw =>
{
    sw.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Divisima Web API - Version 1",
        Description = "Bu proje .net core 7.0 ile geliştirilmiştir",
        TermsOfService = new Uri("http://www.cantacim.com/sozlesme"),
        Contact = new OpenApiContact
        {
            Name = "Ağacan Ergün",
            Email = "agacannn@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT Licence",
            Url = new Uri("http://www.cantacim.com/apilisans"),
        }
    });
    sw.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml"));
});

var app = builder.Build();
app.UseCors("izinverilenler");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
