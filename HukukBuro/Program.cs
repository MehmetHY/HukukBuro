using HukukBuro.Data;
using HukukBuro.Yoneticiler;
using HukukBuro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var baglantiStr = builder.Configuration.GetConnectionString("local")
        ?? throw new KeyNotFoundException("Baglanti metni bulunamadi.");

builder.Services.AddDbContext<VeriTabani>(ayarlar => ayarlar.UseSqlServer(baglantiStr));

builder.Services.AddIdentity<Personel, IdentityRole>(ayarlar =>
{
    ayarlar.SignIn.RequireConfirmedPhoneNumber = false;
    ayarlar.SignIn.RequireConfirmedEmail = false;
    ayarlar.SignIn.RequireConfirmedAccount = false;

    ayarlar.Password.RequireNonAlphanumeric = false;
    ayarlar.Password.RequiredUniqueChars = 0;
    ayarlar.Password.RequireUppercase = false;
    ayarlar.Password.RequireLowercase = false;
    ayarlar.Password.RequiredLength = 3;
    ayarlar.Password.RequireDigit = false;
})
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<VeriTabani>();

builder.Services.ConfigureApplicationCookie(ayarlar =>
{
    ayarlar.LogoutPath = "/personel/cikis";
    ayarlar.LoginPath = "/personel/giris";
    ayarlar.AccessDeniedPath = "/personel/girisengellendi";
});

builder.Services.AddScoped<KisiYoneticisi>();
builder.Services.AddScoped<DosyaYoneticisi>();
builder.Services.AddScoped<PersonelYoneticisi>();
builder.Services.AddScoped<RandevuYoneticisi>();
builder.Services.AddScoped<GorevYoneticisi>();
builder.Services.AddScoped<FinansIslemiYoneticisi>();
builder.Services.AddScoped<DuyuruYoneticisi>();
builder.Services.AddScoped<VeriBaslatici>();

builder.Services.Configure<FormOptions>(o => o.MultipartBodyLengthLimit = 10_000_000);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

using(var scope = app.Services.CreateScope())
{
    var veriBaslatici = scope.ServiceProvider.GetRequiredService<VeriBaslatici>();
    veriBaslatici.BaslangicVerileriniKaydet();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Genel}/{action=Anasayfa}/{id?}");

app.Run();
