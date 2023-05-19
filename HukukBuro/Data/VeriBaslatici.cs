using HukukBuro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HukukBuro.Data;

public class VeriBaslatici
{
    private readonly VeriTabani _veriTabani;
    private readonly UserManager<Personel> _kullaniciYoneticisi;
    private readonly IConfiguration _config;

    public VeriBaslatici(
        VeriTabani veriTabani,
        UserManager<Personel> kullaniciYoneticisi,
        IConfiguration config)
    {
        _veriTabani = veriTabani;
        _kullaniciYoneticisi = kullaniciYoneticisi;
        _config = config;
    }

    public void BaslangicVerileriniKaydet()
    {
        AdminVerileriniGir().GetAwaiter().GetResult();
    }

    private async Task AdminVerileriniGir()
    {
        var adminVar = _veriTabani.UserClaims
            .Any(uc =>
                uc.ClaimType == Sabit.AnaRol.Type &&
                uc.ClaimValue == Sabit.AnaRol.Admin);

        if (adminVar)
            return;

        var adminIsmi = _config["AdminIsmi"] ??
            throw new KeyNotFoundException("Admin ismini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminIsmi)");

        var adminSoyismi = _config["AdminSoyismi"] ??
            throw new KeyNotFoundException("Admin soyismini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminSoyismi)");

        var adminEmail = _config["AdminEmail"] ??
            throw new KeyNotFoundException("Admin emailini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminEmail)");

        var adminSifre = _config["AdminSifre"] ??
            throw new KeyNotFoundException("Admin şifresini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminSifre)");

        var admin = new Personel
        {
            UserName = adminEmail,
            Email = adminEmail,
            Isim = adminIsmi,
            Soyisim = adminSoyismi,
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        var result = await _kullaniciYoneticisi.CreateAsync(admin, adminSifre);

        if (!result.Succeeded)
            throw new InvalidDataException(
                $"Admin hesabı oluştururken bir hata meydana geldi. {string.Join(",", result.Errors.Select(e => e.Description).ToList())}");

        result = await _kullaniciYoneticisi.AddClaimAsync(admin, new Claim(Sabit.AnaRol.Type, Sabit.AnaRol.Admin));

        if (!result.Succeeded)
            throw new Exception(
                $"Admine anarol verirken bir hata meydana geldi. {string.Join(",", result.Errors.Select(e => e.Description).ToList())}");
    }
}
