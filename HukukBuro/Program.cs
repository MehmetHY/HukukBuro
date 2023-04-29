using HukukBuro.Data;
using HukukBuro.Yoneticiler;
using HukukBuro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    ayarlar.Password.RequireUppercase = false;
    ayarlar.Password.RequireLowercase = false;
    ayarlar.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<VeriTabani>();

builder.Services.ConfigureApplicationCookie(ayarlar =>
{
    ayarlar.LogoutPath = "/profil/cikis";
});


builder.Services.AddScoped<KisiYoneticisi>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
