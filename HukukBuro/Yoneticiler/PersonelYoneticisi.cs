using HukukBuro.Araclar;
using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Personeller;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace HukukBuro.Yoneticiler;

public class PersonelYoneticisi
{
    #region Fields
    private readonly VeriTabani _vt;
    private readonly UserManager<Personel> _um;
    private readonly IWebHostEnvironment _env;

    public PersonelYoneticisi(
        VeriTabani vt,
        UserManager<Personel> um,
        IWebHostEnvironment env)
    {
        _vt = vt;
        _um = um;
        _env = env;
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
            new() {Value = Sabit.Yetki.Rol, Text = Sabit.Yetki.RolText},
            new() {Value = Sabit.Yetki.Randevu, Text = Sabit.Yetki.RandevuText},
            new() {Value = Sabit.Yetki.Finans, Text = Sabit.Yetki.FinansText}
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

    #region Personel
    public EkleVM EkleVMGetir()
        => new() { AnaRoller = AnaRolleriGetir(), Yetkiler = YetkileriGetir() };

    public async Task<Sonuc> EkleAsync(EkleVM vm, IFormFile? foto)
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

        if (foto != null)
        {
            var belgeAraci = new BelgeAraci
            {
                Belge = foto,
                GecerliUzantilar = new[] { ".jpg", ".jpeg", ".png", ".webp" },
                Klasor = "foto",
                Root = _env.WebRootPath
            };

            var sonuc = belgeAraci.Onayla();

            if (!sonuc.BasariliMi)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = sonuc.HataMesaji
                };

            sonuc = belgeAraci.Olustur();

            if (!sonuc.BasariliMi)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = sonuc.HataMesaji
                };

            model.FotoUrl = belgeAraci.Url!;
        }
        else
            model.FotoUrl = Sabit.Belge.VarsayilanFotoUrl;

        foreach (var yetki in vm.Yetkiler)
            if (yetki.Checked)
                await _um.AddToRoleAsync(model, yetki.Value);

        await _um.AddClaimAsync(model, new Claim(Sabit.AnaRol.Type, vm.AnaRol));

        return new();
    }

    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _vt.Users
            .Where(u =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                $"{u.Email} {u.UserName} {u.Isim} {u.Soyisim}".Contains(vm.Arama))
            .Select(u => new OzetVM
            {
                Id = u.Id,
                TamIsim = u.TamIsim,
                Email = u.Email!,
                FotoUrl = u.FotoUrl == null ? Sabit.Belge.VarsayilanFotoUrl : u.FotoUrl,
                IlgiliFinansIslemiSayisi = u.IlgiliFinansIslemleri.Count(),
                SorumluDosyaSayisi = u.SorumluDosyalar.Count(),
                SorumluFinansIslemiSayisi = u.SorumluFinansIslemleri.Count(),
                SorumluGorevSayisi = u.SorumluGorevler.Count(),
                SorumluRandevuSayisi = u.SorumluRandevular.Count(),

                Anarol = _vt.UserClaims
                    .Where(uc =>
                        uc.UserId == u.Id &&
                        uc.ClaimType == Sabit.AnaRol.Type)
                    .Select(uc => uc.ClaimValue)
                    .First()!
            });

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz sayfa",
                HataMesaji = $"Sayfa: {vm.Sayfa}, sayfa boyutu: {vm.SayfaBoyutu}"
            };

        vm.Ogeler = await q.SayfaUygula(vm.Sayfa, vm.SayfaBoyutu).ToListAsync();
        vm.ToplamSayfa = await q.ToplamSayfaAsync(vm.SayfaBoyutu);

        return new() { Deger = vm };
    }
}

#endregion
