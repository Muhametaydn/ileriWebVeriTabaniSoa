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
        options.LoginPath = "/Account/Login";  // Giriþ sayfasý
        options.LogoutPath = "/Account/Logout"; // Çýkýþ sayfasý
        options.AccessDeniedPath = "/Account/AccessDenied"; // Eriþim reddedildi sayfasý
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Cookie'nin geçerlilik süresi
        options.SlidingExpiration = true; // Cookie süresini kullanýcý aktifken uzat
        options.Cookie.HttpOnly = true; // Güvenlik için sadece HTTP eriþimine izin ver
        options.Cookie.IsEssential = true; // GDPR uyumu için gerekli iþaretleme
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
