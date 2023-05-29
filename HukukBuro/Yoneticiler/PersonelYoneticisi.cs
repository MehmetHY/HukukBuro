using HukukBuro.Araclar;
using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Personeller;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
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
            new() { Value = Sabit.AnaRol.Onaylanmamis, Text = Sabit.AnaRol.Onaylanmamis },
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

    public async Task<List<CheckboxItem<string>>> YetkileriGetirAsync(string userId)
    {
        var yetkiler = YetkileriGetir();

        var kullaniciYetkileri = await _veriTabani.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        foreach (var yetki in yetkiler)
            yetki.Checked = kullaniciYetkileri.Contains(yetki.Value);

        return yetkiler;
    }

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
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl,
            PhoneNumber = vm.Telefon
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
                FotoUrl = u.FotoUrl,
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

    public async Task<Sonuc<ProfilVM>> ProfilVMGetirAsync(string email, string? id = null)
    {
        var vm = await _veriTabani.Users
            .Where(u => 
                (!string.IsNullOrWhiteSpace(id) && u.Id == id) ||
                (string.IsNullOrWhiteSpace(id) && u.Email == email))
            .Select(u => new ProfilVM
            {
                Id = u.Id,
                Isim = u.Isim,
                Soyisim = u.Soyisim,
                FotoUrl = u.FotoUrl,
                Telefon = u.PhoneNumber,
                Email = u.Email!,

                Anarol = _veriTabani.UserClaims
                    .Where(uc =>
                        uc.UserId == u.Id &&
                        uc.ClaimType == Sabit.AnaRol.Type)
                    .Select(uc => uc.ClaimValue)
                    .First()!,

                FinansIslemleri = _veriTabani.FinansIslemleri
                    .Where(f => f.IslemYapanId == u.Id)
                    .OrderByDescending(f => f.OlusturmaTarihi)
                    .Include(f => f.IslemYapan)
                    .Include(f => f.Kisi)
                    .Include(f => f.Dosya)
                    .Include(f => f.Personel)
                    .Select(f => new ProfilVM.FinansIslemi
                    {
                        Id = f.Id,
                        Aciklama = f.Aciklama,
                        IslemTuru = ((ViewModels.FinansIslemleri.IslemTuru)f.IslemTuru).DisplayName(),
                        KisiId = f.KisiId,
                        Kisi = f.KisiId == null ? null : f.Kisi!.TamIsim,
                        DosyaId = f.DosyaId,
                        Dosya = f.DosyaId == null ? null : f.Dosya!.TamIsim,
                        PersonelId = f.PersonelId,
                        Personel = f.PersonelId == null ? null : f.Personel!.TamIsim,
                        MakbuzKesildi = f.MakbuzKesildiMi,
                        MakbuzNo = f.MakbuzNo,
                        MakbuzTarihi = f.MakbuzTarihi,
                        Miktar = $"{f.Miktar:F2} TL",
                        OdemeTarihi = f.OdemeTarhi,
                        Odendi = f.Odendi,
                        SonOdemeTarihi = f.SonOdemeTarhi
                    })
                    .ToList(),

                Dosyalar = _veriTabani.Dosyalar
                    .Where(d => d.SorumluPersonel.Any(sp => sp.PersonelId == u.Id))
                    .OrderByDescending(d => d.OlusturmaTarihi)
                    .Include(d => d.DosyaDurumu)
                    .Include(d => d.DosyaTuru)
                    .Include(d => d.DosyaKategorisi)
                    .Select(d => new ProfilVM.Dosya
                    {
                        Id = d.Id,
                        DosyaDurumu = d.DosyaDurumu.Isim,
                        DosyaKategorisi = d.DosyaKategorisi.Isim,
                        DosyaTuru = d.DosyaTuru.Isim,
                        TamIsim = d.TamIsim
                    })
                    .ToList(),

                Gorevler = _veriTabani.Gorevler
                    .Where(g => g.SorumluId == u.Id)
                    .OrderByDescending(r => r.OlusturmaTarihi)
                    .Include(g => g.Dosya)
                    .Include(g => g.Kisi)
                    .Include(g => g.Durum)
                    .Select(g => new ProfilVM.Gorev
                    {
                        Id = g.Id,
                        Aciklama = g.Aciklama,
                        BaglantiTuru = ((ViewModels.Gorevler.BaglantiTuru)g.BaglantiTuru).DisplayName(),
                        BitisTarihi = g.BitisTarihi,
                        DosyaId = g.DosyaId,
                        Dosya = g.DosyaId == null ? null : g.Dosya!.TamIsim,
                        Durum = g.Durum.Isim,
                        KisiId = g.KisiId,
                        Kisi = g.KisiId == null ? null : g.Kisi!.TamIsim,
                        Konu = g.Konu,
                        OlusturmaTarihi = g.OlusturmaTarihi
                    })
                    .ToList(),

                Randevular = _veriTabani.Randevular
                    .Where(r => r.SorumluId == u.Id)
                    .OrderByDescending(r => r.OlusturmaTarihi)
                    .Include(r => r.Kisi)
                    .Select(r => new ProfilVM.Randevu
                    {
                        Aciklama = r.Aciklama,
                        Id = r.Id,
                        Kisi = r.Kisi.TamIsim,
                        KisiId = r.KisiId,
                        Konu = r.Konu,
                        Tamamlandi = r.TamamlandiMi,
                        Tarih = r.Tarih
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        vm.KendiProfiliMi =
            string.IsNullOrWhiteSpace(id) ||
            await _veriTabani.Users.AnyAsync(u => u.Id == id && u.Email == email);

        return new() { Deger = vm };
    }

    public async Task<DuzenleVM> DuzenleVMGetirAsync(string email)
        => await _veriTabani.Users
            .Where(u => u.Email == email)
            .Select(u => new DuzenleVM
            {
                Email = email,
                Isim = u.Isim,
                Soyisim = u.Soyisim,
                Telefon = u.PhoneNumber
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
        model.PhoneNumber = vm.Telefon;

        _veriTabani.Users.Update(model);
        await _veriTabani.SaveChangesAsync();

        await _girisYoneticisi.RefreshSignInAsync(model);

        return new();
    }

    public async Task<FotoVM> FotoVMGetirAsync(string email)
        => await _veriTabani.Users
            .Where(u => u.Email == email)
            .Select(u => new FotoVM
            {
                Url = u.FotoUrl
            })
            .FirstAsync();

    public async Task<Sonuc> FotoDuzenleAsync(string email, IFormFile? foto)
    {
        var model = await _veriTabani.Users.FirstAsync(u => u.Email == email);

        var belgeAraci = new BelgeAraci
        {
            Belge = foto,
            GecerliUzantilar = Sabit.Belge.GecerliFotoUzantilari,
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

        if (model.FotoUrl != Sabit.Belge.VarsayilanFotoUrl)
        {
            var eskiFoto = Path.Combine(_env.WebRootPath, model.FotoUrl[1..]);

            if (File.Exists(eskiFoto))
                File.Delete(eskiFoto);
        }

        model.FotoUrl = belgeAraci.Url!;

        _veriTabani.Users.Update(model);
        await _veriTabani.SaveChangesAsync();

        return new();
    }

    public async Task FotoSilAsync(string email)
    {
        var model = await _veriTabani.Users.FirstAsync(u => u.Email == email);

        if (model.FotoUrl == Sabit.Belge.VarsayilanFotoUrl)
            return;

        var eskiFoto = Path.Combine(_env.WebRootPath, model.FotoUrl[1..]);

        if (File.Exists(eskiFoto))
            File.Delete(eskiFoto);

        model.FotoUrl = Sabit.Belge.VarsayilanFotoUrl;
        _veriTabani.Users.Update(model);
        await _veriTabani.SaveChangesAsync();
    }

    public async Task<Sonuc<YetkiDuzenleVM>> YetkiDuzenleVMGetirAsync(string id)
    {
        var vm = await _veriTabani.Users
            .Where(u => u.Id == id)
            .Select(u => new YetkiDuzenleVM
            {
                Id = u.Id,

                Anarol = _veriTabani.UserClaims
                    .Where(uc => uc.UserId == id && uc.ClaimType == Sabit.AnaRol.Type)
                    .Select(uc => uc.ClaimValue)
                    .First()!
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        vm.Anaroller = AnaRolleriGetir();
        vm.Yetkiler = await YetkileriGetirAsync(id);

        return new() { Deger = vm };
    }

    public async Task<Sonuc> YetkiDuzenleAsync(YetkiDuzenleVM vm)
    {
        if (!await _veriTabani.Users.AnyAsync(u => u.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı."
            };

        var anarolClaim = await _veriTabani.UserClaims
            .FirstAsync(uc => uc.UserId == vm.Id && uc.ClaimType == Sabit.AnaRol.Type);

        anarolClaim.ClaimValue = vm.Anarol;
        _veriTabani.UserClaims.Update(anarolClaim);

        var yetkiModelleri = await _veriTabani.UserRoles
            .Where(ur => ur.UserId == vm.Id)
            .ToListAsync();

        var eklenecekYetkiler = new List<IdentityUserRole<string>>();

        foreach (var yetki in vm.Yetkiler)
        {
            var yetkiModeli = yetkiModelleri
                .FirstOrDefault(ym => ym.RoleId == yetki.Value);

            if (yetkiModeli == null && yetki.Checked)
                eklenecekYetkiler.Add(new()
                {
                    UserId = vm.Id,
                    RoleId = yetki.Value
                });

            else if (yetkiModeli != null && !yetki.Checked)
                _veriTabani.UserRoles.Remove(yetkiModeli);
        }

        await _veriTabani.UserRoles.AddRangeAsync(eklenecekYetkiler);
        await _veriTabani.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<OzetVM>> OzetVMGetirAsync(string id)
    {
        var vm = await _veriTabani.Users
            .Where(u => u.Id == id)
            .Include(u => u.IlgiliFinansIslemleri)
            .Include(u => u.SorumluDosyalar)
            .Include(u => u.SorumluFinansIslemleri)
            .Include(u => u.SorumluGorevler)
            .Include(u => u.SorumluRandevular)
            .Select(u => new OzetVM
            {
                Id = u.Id,
                TamIsim = u.TamIsim,
                Email = u.Email!,
                FotoUrl = u.FotoUrl,
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
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        return new() { Deger = vm };
    }

    public async Task<Sonuc<SilVM>> SilVMGetirAsync(string id)
    {
        var vm = await _veriTabani.Users
            .Where(u => u.Id == id)
            .Select(u => new SilVM
            {
                Email = u.Email!,

                Anarol = _veriTabani.UserClaims
                    .Where(uc =>
                        uc.ClaimType == Sabit.AnaRol.Type &&
                        uc.UserId == u.Id)
                    .Select(uc => uc.ClaimValue)
                    .First()!,

                Id = u.Id,
                Isim = u.Isim,
                Soyisim = u.Soyisim,
                Telefon = u.PhoneNumber,
                FotoUrl = u.FotoUrl
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        return new() { Deger = vm };
    }

    public async Task<Sonuc> SilAsync(string id)
    {
        var model = await _veriTabani.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        await BaglantiliDosyalariTemizleAsync(id);
        await BaglantiliFinansIslemleriniTemizleAsync(id);
        await IlgiliFinansIslemleriniTemizleAsync(id);
        await BaglantiliGorevleriTemizleAsync(id);
        await BaglantiliRandevulariTemizleAsync(id);

        _veriTabani.Users.Remove(model);
        await _veriTabani.SaveChangesAsync();

        return new();
    }
    #endregion

    #region Temizle
    public async Task BaglantiliFinansIslemleriniTemizleAsync(string id)
    {
        var modeller = await _veriTabani.FinansIslemleri
            .Where(f => f.IslemYapanId == id)
            .ToListAsync();

        foreach (var model in modeller)
            model.IslemYapanId = null;

        _veriTabani.FinansIslemleri.UpdateRange(modeller);
    }

    public async Task IlgiliFinansIslemleriniTemizleAsync(string id)
    {
        var modeller = await _veriTabani.FinansIslemleri
            .Where(f => f.PersonelBaglantisiVar && f.PersonelId == id)
            .ToListAsync();

        foreach (var model in modeller)
        {
            model.PersonelBaglantisiVar = false;
            model.PersonelId = null;
        }

        _veriTabani.FinansIslemleri.UpdateRange(modeller);
    }

    public async Task BaglantiliDosyalariTemizleAsync(string id)
    {
        var modeller = await _veriTabani.DosyaPersonel
            .Where(dp => dp.PersonelId == id)
            .ToListAsync();

        _veriTabani.DosyaPersonel.RemoveRange(modeller);
    }

    public async Task BaglantiliGorevleriTemizleAsync(string id)
    {
        var modeller = await _veriTabani.Gorevler
            .Where(g => g.SorumluId == id)
            .ToListAsync();

        foreach (var model in modeller)
            model.SorumluId = null;

        _veriTabani.Gorevler.UpdateRange(modeller);
    }

    public async Task BaglantiliRandevulariTemizleAsync(string id)
    {
        var modeller = await _veriTabani.Randevular
            .Where(g => g.SorumluId == id)
            .ToListAsync();

        foreach (var model in modeller)
            model.SorumluId = null;

        _veriTabani.Randevular.UpdateRange(modeller);
    }
    #endregion

    #region Bildirim
    public async Task<int> OkunmamisBildirimSayisiGetirAsync(string email)
        => await _veriTabani.Bildirimler
            .Include(b => b.Personel)
            .Where(b => b.Personel.Email == email && !b.Okundu)
            .CountAsync();

    public async Task<Sonuc<BildirimListeleVM>> BildirimListeleVMGetirAsync(
        BildirimListeleVM vm,
        string email)
    {
        var q = _veriTabani.Bildirimler
            .Include(b => b.Personel)
            .Where(b => b.Personel.Email == email)
            .OrderBy(b => b.Okundu)
            .ThenByDescending(b => b.Tarih)
            .Select(b => new BildirimVM
            {
                Id = b.Id,
                Tarih = b.Tarih,
                Okundu = b.Okundu,
                Mesaj = b.Mesaj,
                Url = b.Url
            });

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Sayfa",
                HataMesaji = $"Sayfa: {vm.Sayfa}, sayfa boyutu: {vm.SayfaBoyutu}"
            };

        vm.Ogeler = await q.SayfaUygula(vm.Sayfa, vm.SayfaBoyutu).ToListAsync();
        vm.ToplamSayfa = await q.ToplamSayfaAsync(vm.SayfaBoyutu);
        await BildirimleriOkunduIsaretle(vm.Ogeler.Select(o => o.Id).ToList());

        return new() { Deger = vm };
    }

    public async Task BildirimleriOkunduIsaretle(IEnumerable<int> ids)
    {
        var modeller = await _veriTabani.Bildirimler
            .Where(b => ids.Contains(b.Id))
            .ToListAsync();

        foreach (var model in modeller)
            model.Okundu = true;

        _veriTabani.Bildirimler.UpdateRange(modeller);
        await _veriTabani.SaveChangesAsync();
    }

    public async Task BildirimleriTemizleAsync(string email)
    {
        var modeller = await _veriTabani.Bildirimler
            .Include(b => b.Personel)
            .Where(b => b.Personel.Email == email)
            .ToListAsync();

        _veriTabani.Bildirimler.RemoveRange(modeller);
        await _veriTabani.SaveChangesAsync();
    }
    #endregion
}

