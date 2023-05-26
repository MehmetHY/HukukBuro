using HukukBuro.Data;
using HukukBuro.Yoneticiler;
using HukukBuro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authorization;
using HukukBuro.Guvenlik.Isleyiciler;
using HukukBuro;
using HukukBuro.Guvenlik.Gereklilikler;

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
    ayarlar.AccessDeniedPath = "/personel/erisimengellendi";
});

builder.Services.AddScoped<KisiYoneticisi>();
builder.Services.AddScoped<DosyaYoneticisi>();
builder.Services.AddScoped<PersonelYoneticisi>();
builder.Services.AddScoped<RandevuYoneticisi>();
builder.Services.AddScoped<GorevYoneticisi>();
builder.Services.AddScoped<FinansIslemiYoneticisi>();
builder.Services.AddScoped<DuyuruYoneticisi>();
builder.Services.AddScoped<VeriBaslatici>();
builder.Services.AddScoped<GenelYonetici>();

builder.Services.AddSingleton<IAuthorizationHandler, DosyaYonetimIslemcisi>();
builder.Services.AddSingleton<IAuthorizationHandler, DuyuruYonetimIslemcisi>();
builder.Services.AddSingleton<IAuthorizationHandler, FinansYonetimIslemcisi>();
builder.Services.AddSingleton<IAuthorizationHandler, GorevYonetimIslemcisi>();
builder.Services.AddSingleton<IAuthorizationHandler, KisiYonetimIslemcisi>();
builder.Services.AddSingleton<IAuthorizationHandler, PersonelYonetimIslemcisi>();
builder.Services.AddSingleton<IAuthorizationHandler, RandevuYonetimIslemcisi>();
builder.Services.AddSingleton<IAuthorizationHandler, RolYonetimIslemcisi>();

builder.Services.Configure<AuthorizationOptions>(ayarlar =>
{
    ayarlar.AddPolicy(
        Sabit.Policy.Dosya,
        policy => policy.AddRequirements(new DosyaYonetimGerekliligi()));

    ayarlar.AddPolicy(
        Sabit.Policy.Duyuru,
        policy => policy.AddRequirements(new DuyuruYonetimGerekliligi()));

    ayarlar.AddPolicy(
        Sabit.Policy.Finans,
        policy => policy.AddRequirements(new FinansYonetimGerekliligi()));

    ayarlar.AddPolicy(
        Sabit.Policy.Gorev,
        policy => policy.AddRequirements(new GorevYonetimGerekliligi()));

    ayarlar.AddPolicy(
        Sabit.Policy.Kisi,
        policy => policy.AddRequirements(new KisiYonetimGerekliligi()));

    ayarlar.AddPolicy(
        Sabit.Policy.Personel,
        policy => policy.AddRequirements(new PersonelYonetimGerekliligi()));

    ayarlar.AddPolicy(
        Sabit.Policy.Randevu,
        policy => policy.AddRequirements(new RandevuYonetimGerekliligi()));

    ayarlar.AddPolicy(
        Sabit.Policy.Rol,
        policy => policy.AddRequirements(new RolYonetimGerekliligi()));
});

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
