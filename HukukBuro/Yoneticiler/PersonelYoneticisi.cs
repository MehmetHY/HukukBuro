using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Personeller;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HukukBuro.Yoneticiler;

public class PersonelYoneticisi
{
    #region Fields
    private readonly VeriTabani _vt;
    private readonly UserManager<Personel> _um;
    private readonly RoleManager<IdentityRole> _rm;

    public PersonelYoneticisi(
        VeriTabani vt, UserManager<Personel> um, RoleManager<IdentityRole> rm)
    {
        _vt = vt;
        _um = um;
        _rm = rm;
    }
    #endregion

    #region Util
    public List<SelectListItem> AnaRolleriGetir()
        => new()
        {
            new() { Value = Sabit.AnaRol.Yonetici, Text = Sabit.AnaRol.Yonetici },
            new() { Value = Sabit.AnaRol.Avukat, Text = Sabit.AnaRol.Avukat },
            new() { Value = Sabit.AnaRol.Calisan, Text = Sabit.AnaRol.Calisan }
        };

    public List<CheckboxItem<string>> YetkileriGetir()
        => new()
        {
            new() {Value = Sabit.Yetki.Kisi, Text = Sabit.Yetki.KisiText},
            new() {Value = Sabit.Yetki.Dosya, Text = Sabit.Yetki.DosyaText},
            new() {Value = Sabit.Yetki.Personel, Text = Sabit.Yetki.PersonelText},
            new() {Value = Sabit.Yetki.Gorev, Text = Sabit.Yetki.GorevText},
            new() {Value = Sabit.Yetki.Duyuru, Text = Sabit.Yetki.DuyuruText},
            new() {Value = Sabit.Yetki.Rol, Text = Sabit.Yetki.RolText}
        };

    public async Task<bool> EmailMevcutMuAsync(string email)
        => await _vt.Users.AnyAsync(u => u.Email == email);

    public async Task<string> AnaRolGetirAsync(string id)
    {
        var anarol = await _vt.UserClaims
            .Where(uc => uc.UserId == id && uc.ClaimType == Sabit.AnaRol.Type)
            .Select(uc => uc.ClaimValue)
            .FirstAsync();

        return anarol ?? Sabit.AnaRol.Calisan;
    }
    #endregion

    public EkleVM EkleVMGetir()
        => new() { AnaRoller = AnaRolleriGetir(), Yetkiler = YetkileriGetir() };

    public async Task<Sonuc> EkleAsync(EkleVM vm)
    {
        if (await EmailMevcutMuAsync(vm.Email))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.Email),
                HataMesaji = "Email zaten alınmış."
            };

        var model = new Personel
        {
            Email = vm.Email,
            UserName = vm.Email,
            Isim = vm.Isim,
            Soyisim = vm.Soyisim
        };

        var result = await _um.CreateAsync(model, vm.Sifre);

        if (!result.Succeeded)
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.Sifre),
                HataMesaji = string.Join(", ", result.Errors.Select(e => e.Description).ToArray())
            };

        foreach (var yetki in vm.Yetkiler)
            if (yetki.Checked)
                await _um.AddToRoleAsync(model, yetki.Value);

        await _um.AddClaimAsync(model, new Claim(Sabit.AnaRol.Type, vm.AnaRol));

        return new();
    }

    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _vt.Users
            .Select(u => new ListeleVM.Oge
            {
                Id = u.Id,
                Email = u.Email!,
                TamIsim = $"{u.Isim} {u.Soyisim}"
            });

        if (!string.IsNullOrWhiteSpace(vm.Arama))
            q = q.Where(u =>
                u.Email.Contains(vm.Arama) || u.TamIsim.Contains(vm.Arama));

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Sayfa",
                HataMesaji = $"Sayfa: {vm.Sayfa}, Sayfa Boyutu: {vm.SayfaBoyutu}"
            };

        vm.ToplamSayfa = await q.SayfaSayisi(vm.SayfaBoyutu);
        vm.Ogeler = await q.SayfaUygula(vm.Sayfa, vm.SayfaBoyutu).ToListAsync();

        foreach (var oge in vm.Ogeler)
            oge.Anarol = await AnaRolGetirAsync(oge.Id);

        return new() { Deger = vm };
    }
}
