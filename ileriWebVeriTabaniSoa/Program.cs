using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ileriWebVeriTabaniSoa.Data;
using ileriWebVeriTabaniSoa.Services;
using ileriWebVeriTabaniSoa.Services.WeatherService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IWeatherService, WeatherService>();
// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Giri� sayfas�
        options.LogoutPath = "/Account/Logout"; // ��k�� sayfas�
        options.AccessDeniedPath = "/Account/AccessDenied"; // Eri�im reddedildi sayfas�
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Cookie'nin ge�erlilik s�resi
        options.SlidingExpiration = true; // Cookie s�resini kullan�c� aktifken uzat
        options.Cookie.HttpOnly = true; // G�venlik i�in sadece HTTP eri�imine izin ver
        options.Cookie.IsEssential = true; // GDPR uyumu i�in gerekli i�aretleme
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use authentication and authorization
app.UseAuthentication(); // Authentication middleware
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
