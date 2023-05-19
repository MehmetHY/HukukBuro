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
    private readonly VeriTabani _veriTabani;
    private readonly UserManager<Personel> _kullaniciYoneticisi;
    private readonly SignInManager<Personel> _girisYoneticisi;
    private readonly IWebHostEnvironment _env;

    public PersonelYoneticisi(
        VeriTabani vt,
        UserManager<Personel> um,
        IWebHostEnvironment env,
        SignInManager<Personel> girisYoneticisi)
    {
        _veriTabani = vt;
        _kullaniciYoneticisi = um;
        _env = env;
        _girisYoneticisi = girisYoneticisi;
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
        => await _veriTabani.Users.AnyAsync(u => u.Email == email);

    public async Task<string> AnaRolGetirAsync(string id)
    {
        var anarol = await _veriTabani.UserClaims
            .Where(uc => uc.UserId == id && uc.ClaimType == Sabit.AnaRol.Type)
            .Select(uc => uc.ClaimValue)
            .FirstAsync();

        return anarol ?? Sabit.AnaRol.Calisan;
    }

    public async Task<bool> OnayliMiAsync(Personel personel)
        => await _veriTabani.UserClaims.Where(uc =>
            uc.UserId == personel.Id && uc.ClaimType == Sabit.AnaRol.Type)
            .Select(uc => uc.ClaimValue)
            .FirstAsync() != Sabit.AnaRol.Onaylanmamis;
    #endregion

    #region Personel
    public KaydolVM KaydolVMGetir() => new();

    public async Task<Sonuc> KaydolAsync(KaydolVM vm)
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
            Soyisim = vm.Soyisim,
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl
        };

        var result = await _kullaniciYoneticisi.CreateAsync(model, vm.Sifre);

        if (!result.Succeeded)
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.Sifre),
                HataMesaji = string.Join(", ", result.Errors.Select(e => e.Description).ToArray())
            };

        await _kullaniciYoneticisi.AddClaimAsync(
            model,
            new Claim(Sabit.AnaRol.Type, Sabit.AnaRol.Onaylanmamis));

        var adminIdleri = await _veriTabani.UserClaims
            .Where(uc =>
                uc.ClaimType == Sabit.AnaRol.Type &&
                uc.ClaimValue == Sabit.AnaRol.Admin)
            .Select(uc => uc.UserId)
            .ToListAsync();

        var yoneticiIdleri =
            (await _kullaniciYoneticisi.GetUsersInRoleAsync(Sabit.Yetki.Personel))
            .Select(p => p.Id)
            .ToList()
            .Union(adminIdleri);

        foreach (var yoneticiId in yoneticiIdleri)
        {
            await _veriTabani.Bildirimler.AddAsync(new Bildirim
            {
                PersonelId = yoneticiId,
                Tarih = DateTime.Now,
                Mesaj = "Yeni bir kullanıcı kaydoldu.",
                Url = $"/personel/profil/{model.Id}"
            });
        }

        await _veriTabani.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _veriTabani.Users
            .Where(u =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                u.Isim.Contains(vm.Arama) ||
                u.Soyisim.Contains(vm.Arama))
            .OrderBy(u => u.Isim)
            .ThenBy(u => u.Soyisim)
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

                Anarol = _veriTabani.UserClaims
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

    public GirisVM GirisVMGetir() => new();

    public async Task<OnaySonuc> GirisAsync(GirisVM vm)
    {
        var model = await _kullaniciYoneticisi.FindByNameAsync(vm.Email);

        if (model == null ||
            !await _kullaniciYoneticisi.CheckPasswordAsync(model, vm.Sifre))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = "Geçersiz email ya da şifre."
            };

        if (!await OnayliMiAsync(model))
            return new()
            {
                BasariliMi = false,
                Onayli = false,
                HataBasligi = "Onaysız Kullanıcı",
                HataMesaji = "Yöneticiler henüz hesabını onaylamadı."
            };

        await _girisYoneticisi.SignInAsync(model, vm.Hatirla);

        return new();
    }

    public async Task<ProfilVM> ProfilVMGetirAsync(string email)
    {
        var vm = await _veriTabani.Users
            .Where(u => u.Email == email)
            .Select(u => new ProfilVM
            {
                Id = u.Id,
                TamIsim = u.TamIsim,
                FotoUrl = u.FotoUrl,
                Email = email,

                Anarol = _veriTabani.UserClaims
                    .Where(uc =>
                        uc.UserId == u.Id &&
                        uc.ClaimType == Sabit.AnaRol.Type)
                    .Select(uc => uc.ClaimValue)
                    .First()!
            })
            .FirstAsync();

        vm.FinansIslemleri = await _veriTabani.FinansIslemleri
            .Where(f => f.IslemYapanId == vm.Id)
            .OrderByDescending(f => f.OlusturmaTarihi)
            .Select(f => new ProfilVM.FinansIslemi
            {
                Id = f.Id,
                Miktar = f.Miktar.ToString("F2"),
                IslemTuru = ((ViewModels.FinansIslemleri.IslemTuru)f.IslemTuru).DisplayName(),
                Odendi = f.Odendi,
                OdemeTarihi = f.Odendi ? f.OdemeTarhi!.Value : f.SonOdemeTarhi!.Value
            })
            .ToListAsync();

        vm.Dosyalar = await _veriTabani.Dosyalar
            .Include(d => d.SorumluPersonel)
            .Include(d => d.DosyaDurumu)
            .Include(d => d.DosyaKategorisi)
            .Include(d => d.DosyaTuru)
            .Where(d => d.SorumluPersonel.Any(sp => sp.PersonelId == vm.Id))
            .OrderByDescending(d => d.OlusturmaTarihi)
            .Select(d => new ProfilVM.Dosya
            {
                Id = d.Id,
                TamIsim = d.TamIsim,
                DosyaDurumu = d.DosyaDurumu.Isim,
                DosyaKategorisi = d.DosyaKategorisi.Isim,
                DosyaTuru = d.DosyaTuru.Isim
            })
            .ToListAsync();

        vm.Gorevler = await _veriTabani.Gorevler
            .Where(g => g.SorumluId == vm.Id)
            .Include(g => g.Durum)
            .OrderByDescending(g => g.OlusturmaTarihi)
            .Select(g => new ProfilVM.Gorev
            {
                Id = g.Id,
                Konu = g.Konu,
                Durum = g.Durum.Isim,
                BitisTarihi = g.BitisTarihi
            })
            .ToListAsync();

        vm.Randevular = await _veriTabani.Randevular
            .Where(r => r.SorumluId == vm.Id)
            .Include(r => r.Kisi)
            .OrderByDescending(r => r.OlusturmaTarihi)
            .Select(r => new ProfilVM.Randevu
            {
                Id = r.Id,
                Kisi = r.Kisi.TamIsim,
                Konu = r.Konu,
                Tamamlandi = r.TamamlandiMi,
                Tarih = r.Tarih
            })
            .ToListAsync();

        return vm;
    }

    public async Task<DuzenleVM> DuzenleVMGetirAsync(string email)
        => await _veriTabani.Users
            .Where(u => u.Email == email)
            .Select(u => new DuzenleVM
            {
                Email = email,
                Isim = u.Isim,
                Soyisim = u.Soyisim
            })
            .FirstAsync();

    public async Task<Sonuc> DuzenleAsync(DuzenleVM vm, string email)
    {
        var model = await _veriTabani.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"email: {email} bulunamadı"
            };

        if (model.Email != vm.Email)
        {
            if (await _veriTabani.Users.AnyAsync(u => u.Id != model.Id && u.Email == vm.Email))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.Email),
                    HataMesaji = $"Zaten mevcut."
                };

            model.Email = vm.Email;
            model.UserName = model.Email;
            model.NormalizedEmail = vm.Email.ToUpper();
            model.NormalizedUserName = model.NormalizedEmail;
        }

        if (vm.SifreDegistir)
        {
            if (string.IsNullOrWhiteSpace(vm.EskiSifre))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.EskiSifre),
                    HataMesaji = $"Gerekli."
                };

            if (string.IsNullOrWhiteSpace(vm.YeniSifre))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.YeniSifre),
                    HataMesaji = $"Gerekli."
                };

            if (string.IsNullOrWhiteSpace(vm.YeniSifreTekrar))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.YeniSifreTekrar),
                    HataMesaji = $"Gerekli."
                };

            var result = await _kullaniciYoneticisi.ChangePasswordAsync(
                model, vm.EskiSifre, vm.YeniSifre);

            if (!result.Succeeded)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.YeniSifre),
                    HataMesaji = $"Başarısız."
                };
        }

        model.Isim = vm.Isim;
        model.Soyisim = vm.Soyisim;

        _veriTabani.Users.Update(model);
        await _veriTabani.SaveChangesAsync();

        await _girisYoneticisi.RefreshSignInAsync(model);

        return new();
    }
    #endregion
}

