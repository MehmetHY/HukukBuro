using HukukBuro.Araclar;
using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Dosyalar;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class DosyaYoneticisi
{
    #region Fields
    private readonly VeriTabani _vt;
    private readonly IWebHostEnvironment _env;

    public DosyaYoneticisi(VeriTabani vt, IWebHostEnvironment env)
    {
        _vt = vt;
        _env = env;
    }
    #endregion

    #region Dosya
    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _vt.Dosyalar
            .AsNoTracking()
            .Include(d => d.DosyaTuru)
            .Include(d => d.DosyaKategorisi)
            .Include(d => d.DosyaDurumu)
            .Include(d => d.IlgiliGorevler)
            .Include(d => d.Durusmalar)
            .Include(d => d.Belgeler)
            .Include(d => d.IlgiliFinansIslemleri)
            .Select(d => new ListeleVM.Oge
            {
                Id = d.Id,
                DosyaNo = d.DosyaNo,
                BuroNo = d.BuroNo,
                Konu = d.Konu,
                DosyaTuru = d.DosyaTuru.Isim,
                DosyaKategorisi = d.DosyaKategorisi.Isim,
                DosyaDurumu = d.DosyaDurumu.Isim,
                AcilisTarihi = d.AcilisTarihi,
                GorevSayisi = d.IlgiliGorevler.Count,
                DurusmaSayisi = d.Durusmalar.Count,
                BelgeSayisi = d.Belgeler.Count,
                FinansSayisi = d.IlgiliFinansIslemleri.Count
            });

        if (!string.IsNullOrWhiteSpace(vm.Arama))
            q = q.Where(d =>
                d.DosyaNo.ToString().Contains(vm.Arama) ||
                d.BuroNo.Contains(vm.Arama) ||
                d.Konu.Contains(vm.Arama) ||
                d.DosyaTuru.Contains(vm.Arama) ||
                d.DosyaKategorisi.Contains(vm.Arama) ||
                d.DosyaDurumu.Contains(vm.Arama));

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Sayfa",
                HataMesaji = $"Sayfa: {vm.Sayfa}, Sayfa Boyutu: {vm.SayfaBoyutu}"
            };

        vm.ToplamSayfa = await q.ToplamSayfaAsync(vm.SayfaBoyutu);

        vm.Ogeler = await q
            .SayfaUygula(vm.Sayfa, vm.SayfaBoyutu)
            .ToListAsync();

        return new() { Deger = vm };
    }

    public async Task<List<SelectListItem>> DosyaTurleriGetirAsync()
        => await _vt.DosyaTurleri
            .AsNoTracking()
            .Select(dt => new SelectListItem
            {
                Value = dt.Id.ToString(),
                Text = dt.Isim
            })
            .ToListAsync();

    public async Task<List<SelectListItem>> DosyaKategorileriGetirAsync()
        => await _vt.DosyaKategorileri
            .AsNoTracking()
            .Select(dk => new SelectListItem
            {
                Value = dk.Id.ToString(),
                Text = dk.Isim
            })
            .ToListAsync();

    public async Task<List<SelectListItem>> DosyaDurumlariGetirAsync()
        => await _vt.DosyaDurumu
            .AsNoTracking()
            .Select(dd => new SelectListItem
            {
                Value = dd.Id.ToString(),
                Text = dd.Isim
            })
            .ToListAsync();

    public async Task<EkleVM> EkleVMGetirAsync()
        => new()
        {
            DosyaTurleri = await DosyaTurleriGetirAsync(),
            DosyaKategorileri = await DosyaKategorileriGetirAsync(),
            DosyaDurumlari = await DosyaDurumlariGetirAsync()
        };

    public async Task EkleAsync(EkleVM vm)
    {
        var model = new Dosya
        {
            DosyaNo = vm.DosyaNo,
            BuroNo = vm.BuroNo,
            Konu = vm.Konu,
            Aciklama = vm.Aciklama,
            DosyaTuruId = vm.DosyaTuruId,
            DosyaKategorisiId = vm.DosyaKategorisiId,
            DosyaDurumuId = vm.DosyaDurumuId,
            Mahkeme = vm.Mahkeme,
            AcilisTarihi = vm.AcilisTarihi
        };

        model.KararBilgileri = new() { Dosya = model };
        model.BolgeAdliyeMahkemesiBilgileri = new() { Dosya = model };
        model.TemyizBilgileri = new() { Dosya = model };
        model.KararDuzeltmeBilgileri = new() { Dosya = model };
        model.KesinlesmeBilgileri = new() { Dosya = model };

        await _vt.Dosyalar.AddAsync(model);
        await _vt.SaveChangesAsync();
    }

    public async Task<Sonuc<OzetVM>> OzetVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _vt.Dosyalar
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new OzetVM
            {
                Id = d.Id,
                DosyaNo = d.DosyaNo,
                BuroNo = d.BuroNo,
                Konu = d.Konu,
                Aciklama = d.Aciklama ?? string.Empty,
                DosyaTuru = d.DosyaTuru.Isim,
                DosyaKategorisi = d.DosyaKategorisi.Isim,
                DosyaDurumu = d.DosyaDurumu.Isim,
                Mahkeme = d.Mahkeme ?? string.Empty,
                AcilisTarihi = d.AcilisTarihi
            })
            .FirstAsync();

        vm.KarsiTaraf = await TarafGetirAsync(id, true);
        vm.MuvekkilTaraf = await TarafGetirAsync(id, false);
        vm.SorumluPersonel = await OzetPersonelGetirAsync(id);
        vm.DosyaBaglantilari = await OzetDosyaBaglantilariGetirAsync(id);
        vm.Durusmalar = await OzetDurusmalariGetirAsync(id);

        return new() { Deger = vm };
    }

    public async Task<Sonuc<DuzenleVM>> DuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _vt.Dosyalar
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DuzenleVM
            {
                Id = d.Id,
                DosyaNo = d.DosyaNo,
                BuroNo = d.BuroNo,
                Konu = d.Konu,
                Aciklama = d.Aciklama ?? string.Empty,
                DosyaTuruId = d.DosyaTuruId,
                DosyaKategorisiId = d.DosyaKategorisiId,
                DosyaDurumuId = d.DosyaDurumuId,
                Mahkeme = d.Mahkeme ?? string.Empty,
                AcilisTarihi = d.AcilisTarihi
            })
            .FirstAsync();

        vm.DosyaTurleri = await DosyaTurleriGetirAsync();
        vm.DosyaKategorileri = await DosyaKategorileriGetirAsync();
        vm.DosyaDurumlari = await DosyaDurumlariGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc> DuzenleAsync(DuzenleVM vm)
    {
        var model = await _vt.Dosyalar.FirstOrDefaultAsync(d => d.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"Geçersiz id: {vm.Id}"
            };

        model.Id = vm.Id;
        model.DosyaNo = vm.DosyaNo;
        model.BuroNo = vm.BuroNo;
        model.Konu = vm.Konu;
        model.Aciklama = vm.Aciklama;
        model.DosyaTuruId = vm.DosyaTuruId;
        model.DosyaKategorisiId = vm.DosyaKategorisiId;
        model.DosyaDurumuId = vm.DosyaDurumuId;
        model.Mahkeme = vm.Mahkeme;
        model.AcilisTarihi = vm.AcilisTarihi;

        _vt.Dosyalar.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }
    
    public async Task<Sonuc<SilVM>> SilVMGetirAsync(int id)
    {
        var vm = await _vt.Dosyalar
            .Where(d => d.Id == id)
            .Include(d => d.DosyaTuru)
            .Include(d => d.DosyaKategorisi)
            .Include(d => d.DosyaDurumu)
            .Select(d => new SilVM
            {
                Id = d.Id,
                Aciklama = d.Aciklama,
                BuroNo = d.BuroNo,
                Konu = d.Konu,
                DosyaDurumu = d.DosyaDurumu.Isim,
                DosyaKategorisi = d.DosyaKategorisi.Isim,
                DosyaNo = d.DosyaNo,
                DosyaTuru = d.DosyaTuru.Isim
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

    public async Task<Sonuc> SilAsync(int id)
    {
        var model = await _vt.Dosyalar.FirstOrDefaultAsync(d => d.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        await PersonelBaglantlariniTemizleAsync(id);
        await DosyaBaglantilariniTemizleAsync(id);
        await GorevBaglantilariniTemizleAsync(id);
        await BelgeleriTemizleAsync(id);
        await FinansBaglantilariniTemizleAsync(id);
        await KararBilgileriniTemizleAsync(id);

        _vt.Dosyalar.Remove(model);
        await _vt.SaveChangesAsync();

        return new();
    }
    #endregion

    #region Taraf
    public async Task<Sonuc<TarafEkleVM>> TarafEkleVMGetirAsync(int dosyaId)
    {
        if (dosyaId < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == dosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"Id: {dosyaId} bulunamadı."
            };

        var vm = new TarafEkleVM
        {
            DosyaId = dosyaId,
            Kisiler = await KisileriGetirAsync(),
            TarafTurleri = await TarafTurleriGetirAsync()
        };

        return new() { Deger = vm };
    }

    public async Task<Sonuc> TarafEkleAsync(TarafEkleVM vm)
    {
        if (vm.DosyaId < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"Id: {vm.DosyaId} bulunamadı."
            };

        var model = new TarafKisi
        {
            DosyaId = vm.DosyaId,
            KisiId = vm.KisiId,
            KarsiTaraf = vm.KarsiTarafMi,
            TarafTuruId = vm.TarafTuruId
        };

        await _vt.TarafKisiler.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<List<SelectListItem>> TarafTurleriGetirAsync()
        => await _vt.TarafTurleri
        .AsNoTracking()
        .Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Isim
        })
        .ToListAsync();

    public async Task<List<SelectListItem>> KisileriGetirAsync()
        => await _vt.Kisiler
        .AsNoTracking()
        .Select(k => new SelectListItem
        {
            Value = k.Id.ToString(),
            Text = k.TuzelMi ? k.SirketIsmi! : $"{k.Isim} {k.Soyisim}"
        })
        .ToListAsync();

    public async Task<List<OzetVM.Taraf>> TarafGetirAsync(
        int dosyaId, bool karsiTaraf)
        => await _vt.TarafKisiler
            .AsNoTracking()
            .Where(t => t.DosyaId == dosyaId && t.KarsiTaraf == karsiTaraf)
            .Include(t => t.Kisi)
            .Include(t => t.TarafTuru)
            .Select(t => new OzetVM.Taraf
            {
                Id = t.Id,

                Isim = t.Kisi.TuzelMi ?
                    t.Kisi.SirketIsmi! :
                    $"{t.Kisi.Isim} {t.Kisi.Soyisim}",

                TarafTuru = t.TarafTuru.Isim
            })
            .ToListAsync();

    public async Task<Sonuc<TarafDuzenleVM>> TarafDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.TarafKisiler.AnyAsync(t => t.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _vt.TarafKisiler
            .Where(t => t.Id == id)
            .Select(t => new TarafDuzenleVM
            {
                Id = id,
                DosyaId = t.DosyaId,
                KisiId = t.KisiId,
                KarsiTarafMi = t.KarsiTaraf,
                TarafTuruId = t.TarafTuruId
            })
            .FirstAsync();

        vm.Kisiler = await KisileriGetirAsync();
        vm.TarafTurleri = await TarafTurleriGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> TarafDuzenleAsync(TarafDuzenleVM vm)
    {
        var model = await _vt.TarafKisiler.FirstOrDefaultAsync(t => t.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı."
            };

        model.KisiId = vm.KisiId;
        model.TarafTuruId = vm.TarafTuruId;
        model.KarsiTaraf = vm.KarsiTarafMi;

        _vt.TarafKisiler.Update(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = model.DosyaId };
    }

    public async Task<Sonuc<TarafSilVM>> TarafSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.TarafKisiler.AnyAsync(t => t.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _vt.TarafKisiler
            .Where(t => t.Id == id)
            .Include(t => t.Kisi)
            .Include(t => t.TarafTuru)
            .Select(t => new TarafSilVM
            {
                Id = id,
                DosyaId = t.DosyaId,

                Isim = t.Kisi.TuzelMi ?
                    t.Kisi.SirketIsmi! :
                    $"{t.Kisi.Isim} {t.Kisi.Soyisim}",

                KarsiTarafMi = t.KarsiTaraf,
                TarafTuru = t.TarafTuru.Isim
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> TarafSilAsync(int id)
    {
        var model = await _vt.TarafKisiler.FirstOrDefaultAsync(t => t.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var dosyaId = model.DosyaId;
        _vt.TarafKisiler.Remove(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }
    #endregion

    #region Personel
    public async Task<Sonuc<PersonelDuzenleVM>> PersonelDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var vm = new PersonelDuzenleVM { Id = id };

        vm.PersonelListe = await _vt.Users
            .Include(p => p.SorumluDosyalar)
            .Select(p => new CheckboxItem<string>
            {
                Checked = p.SorumluDosyalar.Any(dp => dp.DosyaId == id),
                Value = p.Id,
                Text = $"{p.Isim} {p.Soyisim}"
            })
            .ToListAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> PersonelDuzenleAsync(PersonelDuzenleVM vm)
    {
        if (vm.Id < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"Id: {vm.Id} bulunamadı."
            };

        var personeller = await _vt.Users
            .Include(p => p.SorumluDosyalar)
            .ToListAsync();

        foreach (var personel in personeller)
        {
            var item = vm.PersonelListe.FirstOrDefault(p => p.Value == personel.Id);

            if (item == null)
                continue;

            var dosyaPersonel = await _vt.DosyaPersonel
                .FirstOrDefaultAsync(dp =>
                    dp.DosyaId == vm.Id &&
                    dp.PersonelId == item.Value);

            if (dosyaPersonel != null && !item.Checked)
                _vt.DosyaPersonel.Remove(dosyaPersonel);
            else if (dosyaPersonel == null && item.Checked)
                await _vt.DosyaPersonel.AddAsync(new()
                {
                    DosyaId = vm.Id,
                    PersonelId = item.Value
                });
        }

        await _vt.SaveChangesAsync();

        return new() { Deger = vm.Id };
    }

    public async Task<List<OzetVM.Personel>> OzetPersonelGetirAsync(int id)
    {
        var personel = await _vt.DosyaPersonel
            .Where(dp => dp.DosyaId == id)
            .Select(dp => new OzetVM.Personel
            {
                TamIsim = $"{dp.Personel.Isim} {dp.Personel.Soyisim}",

                AnaRol = _vt.UserClaims
                    .Where(uc =>
                        uc.UserId == dp.PersonelId &&
                        uc.ClaimType == Sabit.AnaRol.Type)
                    .Select(uc => uc.ClaimValue)
                    .First() ?? Sabit.AnaRol.Calisan
            })
            .ToListAsync();

        return personel;
    }
    #endregion

    #region DosyaBaglantisi
    public async Task<Sonuc<DosyaBaglantisiEkleVM>> DosyaBaglantisiEkleVMGetirAsync(
        int dosyaId)
    {
        if (dosyaId < 1 ||
            !await _vt.Dosyalar.AnyAsync(d => d.Id == dosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {dosyaId} bulunamadı."
            };

        var vm = new DosyaBaglantisiEkleVM
        {
            DosyaId = dosyaId,
            Dosyalar = await DosyalariGetirAsync()
        };

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DosyaBaglantisiEkleAsync(DosyaBaglantisiEkleVM vm)
    {
        if (vm.DosyaId < 1 ||
            !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.DosyaId} bulunamadı."
            };

        if (vm.IlgiliDosyaId < 1 ||
            !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.IlgiliDosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.IlgiliDosyaId),
                HataMesaji = $"id: {vm.IlgiliDosyaId} bulunamadı."
            };

        var model = new DosyaBaglantisi
        {
            DosyaId = vm.DosyaId,
            IlgiliDosyaId = vm.IlgiliDosyaId,
            Aciklama = vm.Aciklama
        };

        await _vt.DosyaBaglantilari.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }

    public async Task<List<SelectListItem>> DosyalariGetirAsync()
        => await _vt.Dosyalar
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.DosyaNo} {d.BuroNo} {d.Konu}"
            })
            .ToListAsync();

    public async Task<List<OzetVM.Baglanti>> OzetDosyaBaglantilariGetirAsync(int id)
        => await _vt.DosyaBaglantilari
        .Include(db => db.IlgiliDosya)
        .Where(db => db.DosyaId == id)
        .Select(db => new OzetVM.Baglanti
        {
            Id = db.Id,
            IlgiliDosyaId = db.IlgiliDosyaId,
            DosyaNo = db.IlgiliDosya.DosyaNo,
            BuroNo = db.IlgiliDosya.BuroNo,
            Konu = db.IlgiliDosya.Konu
        })
        .ToListAsync();

    public async Task<Sonuc<DosyaBaglantisiDuzenleVM>>
        DosyaBaglantisiDuzenleVMGetirAsync(int id)
    {
        if (id < 1 ||
            !await _vt.DosyaBaglantilari.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _vt.DosyaBaglantilari
            .Select(db => new DosyaBaglantisiDuzenleVM
            {
                Id = id,
                Aciklama = db.Aciklama,
                DosyaId = db.DosyaId,
                IlgiliDosyaId = db.IlgiliDosyaId
            })
            .FirstAsync();

        vm.Dosyalar = await DosyalariGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DosyaBaglantisiDuzenleAsync(
        DosyaBaglantisiDuzenleVM vm)
    {
        var model = await _vt.DosyaBaglantilari
            .FirstOrDefaultAsync(db => db.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı."
            };

        if (vm.DosyaId < 1 ||
            !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.DosyaId),
                HataMesaji = $"id: {vm.DosyaId} bulunamadı."
            };

        if (vm.IlgiliDosyaId < 1 ||
            !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.IlgiliDosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.IlgiliDosyaId),
                HataMesaji = $"id: {vm.IlgiliDosyaId} bulunamadı."
            };

        model.Aciklama = vm.Aciklama;
        model.DosyaId = vm.DosyaId;
        model.IlgiliDosyaId = vm.IlgiliDosyaId;
        _vt.DosyaBaglantilari.Update(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }

    public async Task<Sonuc<DosyaBaglantisiSilVM>> DosyaBaglantisiSilVMGetirAsync(
        int id)
    {
        if (id < 1 ||
            !await _vt.DosyaBaglantilari.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _vt.DosyaBaglantilari
            .Include(db => db.IlgiliDosya)
            .Select(db => new DosyaBaglantisiSilVM
            {
                Id = id,
                Aciklama = db.Aciklama,
                DosyaId = db.DosyaId,
                IlgiliDosya = $"{db.IlgiliDosya.DosyaNo} {db.IlgiliDosya.BuroNo} {db.IlgiliDosya.Konu}"
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DosyaBaglantisiSilAsync(int id)
    {
        var model = await _vt.DosyaBaglantilari
            .FirstOrDefaultAsync(db => db.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var dosyaId = model.DosyaId;
        _vt.DosyaBaglantilari.Remove(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }
    #endregion

    #region Karar
    public async Task<Sonuc<KararVM>> KararVMGetirAsync(int dosyaId)
    {
        if (dosyaId < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == dosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {dosyaId} bulunamadı."
            };

        var vm = await _vt.Dosyalar
            .Include(d => d.KararBilgileri)
            .Include(d => d.BolgeAdliyeMahkemesiBilgileri)
            .Include(d => d.TemyizBilgileri)
            .Include(d => d.KararDuzeltmeBilgileri)
            .Include(d => d.KesinlesmeBilgileri)
            .Select(d => new KararVM
            {
                DosyaId = dosyaId,

                KararBilgileri = new()
                {
                    KararNo = d.KararBilgileri!.KararNo,
                    KararOzeti = d.KararBilgileri.KararOzeti,
                    KararTarihi = d.KararBilgileri.KararTarihi,
                    TebligTarihi = d.KararBilgileri.TebligTarihi
                },

                BolgeAdliyeMahkemesiBilgileri = new()
                {
                    Aciklama = d.BolgeAdliyeMahkemesiBilgileri!.Aciklama,
                    EsasNo = d.BolgeAdliyeMahkemesiBilgileri.EsasNo,
                    GondermeTarihi = d.BolgeAdliyeMahkemesiBilgileri.GondermeTarihi,
                    KararNo = d.BolgeAdliyeMahkemesiBilgileri.KararNo,
                    KararOzeti = d.BolgeAdliyeMahkemesiBilgileri.KararOzeti,
                    KararTarihi = d.BolgeAdliyeMahkemesiBilgileri.KararTarihi,
                    Mahkeme = d.BolgeAdliyeMahkemesiBilgileri.Mahkeme,
                    TebligTarihi = d.BolgeAdliyeMahkemesiBilgileri.TebligTarihi
                },

                TemyizBilgileri = new()
                {
                    Aciklama = d.TemyizBilgileri!.Aciklama,
                    EsasNo = d.TemyizBilgileri.EsasNo,
                    GondermeTarihi = d.TemyizBilgileri.GondermeTarihi,
                    KararNo = d.TemyizBilgileri.KararNo,
                    KararOzeti = d.TemyizBilgileri.KararOzeti,
                    KararTarihi = d.TemyizBilgileri.KararTarihi,
                    Mahkeme = d.TemyizBilgileri.Mahkeme,
                    TebligTarihi = d.TemyizBilgileri.TebligTarihi
                },

                KararDuzeltmeBilgileri = new()
                {
                    Aciklama = d.KararDuzeltmeBilgileri!.Aciklama,
                    EsasNo = d.KararDuzeltmeBilgileri.EsasNo,
                    GondermeTarihi = d.KararDuzeltmeBilgileri.GondermeTarihi,
                    KararNo = d.KararDuzeltmeBilgileri.KararNo,
                    KararOzeti = d.KararDuzeltmeBilgileri.KararOzeti,
                    KararTarihi = d.KararDuzeltmeBilgileri.KararTarihi,
                    Mahkeme = d.KararDuzeltmeBilgileri.Mahkeme,
                    TebligTarihi = d.KararDuzeltmeBilgileri.TebligTarihi
                },

                KesinlesmeBilgileri = new()
                {
                    KararOzeti = d.KesinlesmeBilgileri!.KararOzeti,
                    KesinlesmeTarihi = d.KesinlesmeBilgileri.KesinlesmeTarihi
                }
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> KararDuzenleAsync(KararVM vm)
    {
        var dosya = await _vt.Dosyalar
            .Include(d => d.KararBilgileri)
            .Include(d => d.BolgeAdliyeMahkemesiBilgileri)
            .Include(d => d.TemyizBilgileri)
            .Include(d => d.KararDuzeltmeBilgileri)
            .Include(d => d.KesinlesmeBilgileri)
            .FirstOrDefaultAsync(d => d.Id == vm.DosyaId);

        if (dosya == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"Id: {vm.DosyaId} bulunamadı."
            };

        dosya.KararBilgileri!.KararNo = vm.KararBilgileri!.KararNo;
        dosya.KararBilgileri.KararOzeti = vm.KararBilgileri.KararOzeti;
        dosya.KararBilgileri.KararTarihi = vm.KararBilgileri.KararTarihi;
        dosya.KararBilgileri.TebligTarihi = vm.KararBilgileri.TebligTarihi;

        dosya.BolgeAdliyeMahkemesiBilgileri!.Aciklama = vm.BolgeAdliyeMahkemesiBilgileri!.Aciklama;
        dosya.BolgeAdliyeMahkemesiBilgileri.EsasNo = vm.BolgeAdliyeMahkemesiBilgileri.EsasNo;
        dosya.BolgeAdliyeMahkemesiBilgileri.GondermeTarihi = vm.BolgeAdliyeMahkemesiBilgileri.GondermeTarihi;
        dosya.BolgeAdliyeMahkemesiBilgileri.KararNo = vm.BolgeAdliyeMahkemesiBilgileri.KararNo;
        dosya.BolgeAdliyeMahkemesiBilgileri.KararOzeti = vm.BolgeAdliyeMahkemesiBilgileri.KararOzeti;
        dosya.BolgeAdliyeMahkemesiBilgileri.KararTarihi = vm.BolgeAdliyeMahkemesiBilgileri.KararTarihi;
        dosya.BolgeAdliyeMahkemesiBilgileri.Mahkeme = vm.BolgeAdliyeMahkemesiBilgileri.Mahkeme;
        dosya.BolgeAdliyeMahkemesiBilgileri.TebligTarihi = vm.BolgeAdliyeMahkemesiBilgileri.TebligTarihi;

        dosya.TemyizBilgileri!.Aciklama = vm.TemyizBilgileri!.Aciklama;
        dosya.TemyizBilgileri.EsasNo = vm.TemyizBilgileri.EsasNo;
        dosya.TemyizBilgileri.GondermeTarihi = vm.TemyizBilgileri.GondermeTarihi;
        dosya.TemyizBilgileri.KararNo = vm.TemyizBilgileri.KararNo;
        dosya.TemyizBilgileri.KararOzeti = vm.TemyizBilgileri.KararOzeti;
        dosya.TemyizBilgileri.KararTarihi = vm.TemyizBilgileri.KararTarihi;
        dosya.TemyizBilgileri.Mahkeme = vm.TemyizBilgileri.Mahkeme;
        dosya.TemyizBilgileri.TebligTarihi = vm.TemyizBilgileri.TebligTarihi;

        dosya.KararDuzeltmeBilgileri!.Aciklama = vm.KararDuzeltmeBilgileri!.Aciklama;
        dosya.KararDuzeltmeBilgileri.EsasNo = vm.KararDuzeltmeBilgileri.EsasNo;
        dosya.KararDuzeltmeBilgileri.GondermeTarihi = vm.KararDuzeltmeBilgileri.GondermeTarihi;
        dosya.KararDuzeltmeBilgileri.KararNo = vm.KararDuzeltmeBilgileri.KararNo;
        dosya.KararDuzeltmeBilgileri.KararOzeti = vm.KararDuzeltmeBilgileri.KararOzeti;
        dosya.KararDuzeltmeBilgileri.KararTarihi = vm.KararDuzeltmeBilgileri.KararTarihi;
        dosya.KararDuzeltmeBilgileri.Mahkeme = vm.KararDuzeltmeBilgileri.Mahkeme;
        dosya.KararDuzeltmeBilgileri.TebligTarihi = vm.KararDuzeltmeBilgileri.TebligTarihi;

        dosya.KesinlesmeBilgileri!.KararOzeti = vm.KesinlesmeBilgileri!.KararOzeti;
        dosya.KesinlesmeBilgileri.KesinlesmeTarihi = vm.KesinlesmeBilgileri.KesinlesmeTarihi;

        _vt.KararBilgileri.Update(dosya.KararBilgileri);
        _vt.BolgeAdliyeMahkemesiBilgileri.Update(dosya.BolgeAdliyeMahkemesiBilgileri);
        _vt.TemyizBilgileri.Update(dosya.TemyizBilgileri);
        _vt.KararDuzeltmeBilgileri.Update(dosya.KararDuzeltmeBilgileri);
        _vt.KesinlesmeBilgileri.Update(dosya.KesinlesmeBilgileri);
        await _vt.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }
    #endregion

    #region Durusma
    public async Task<List<OzetVM.Durusma>> OzetDurusmalariGetirAsync(int id)
        => await _vt.Durusmalar
        .Where(d => d.DosyaId == id)
        .Include(d => d.AktiviteTuru)
        .Select(d => new OzetVM.Durusma
        {
            Id = d.Id,
            AktiviteTuru = d.AktiviteTuru.Isim,
            Tarih = d.Tarih,
            Aciklama = d.Aciklama
        })
        .ToListAsync();

    public async Task<Sonuc<DurusmaEkleVM>> DurusmaEkleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var vm = new DurusmaEkleVM
        {
            DosyaId = id,
            AktiviteTurleri = await DurusmaAktiviteTurleriGetirAsync()
        };

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DurusmaEkleAsync(DurusmaEkleVM vm)
    {
        if (vm.DosyaId < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"Id: {vm.DosyaId} bulunamadı."
            };

        var model = new Durusma
        {
            DosyaId = vm.DosyaId,
            Aciklama = vm.Aciklama,
            AktiviteTuruId = vm.AktiviteTuruId,
            Tarih = vm.Tarih
        };

        await _vt.Durusmalar.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }
    public async Task<Sonuc<DurusmaDuzenleVM>> DurusmaDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Durusmalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var vm = await _vt.Durusmalar
            .Where(d => d.Id == id)
            .Select(d => new DurusmaDuzenleVM
            {
                Id = d.Id,
                DosyaId = d.DosyaId,
                Aciklama = d.Aciklama,
                AktiviteTuruId = d.AktiviteTuruId,
                Tarih = d.Tarih
            })
            .FirstAsync();

        vm.AktiviteTurleri = await DurusmaAktiviteTurleriGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DurusmaDuzenleAsync(DurusmaDuzenleVM vm)
    {
        var model = await _vt.Durusmalar.FirstOrDefaultAsync(d => d.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"Id: {vm.Id} bulunamadı."
            };

        model.Aciklama = vm.Aciklama;
        model.AktiviteTuruId = vm.AktiviteTuruId;
        model.Tarih = vm.Tarih;

        _vt.Durusmalar.Update(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = model.DosyaId };
    }

    public async Task<Sonuc<DurusmaSilVM>> DurusmaSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Durusmalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var vm = await _vt.Durusmalar
            .Where(d => d.Id == id)
            .Include(d => d.AktiviteTuru)
            .Select(d => new DurusmaSilVM
            {
                Id = d.Id,
                DosyaId = d.DosyaId,
                Aciklama = d.Aciklama,
                AktiviteTuru = d.AktiviteTuru.Isim,
                Tarih = d.Tarih
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DurusmaSilAsync(int id)
    {
        var model = await _vt.Durusmalar.FirstOrDefaultAsync(d => d.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var dosyaId = model.DosyaId;

        _vt.Durusmalar.Remove(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }

    public async Task<List<SelectListItem>> DurusmaAktiviteTurleriGetirAsync()
        => await _vt.DurusmaAktiviteTurleri
        .Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Isim
        })
        .ToListAsync();
    #endregion

    #region DosyaBelgesi
    public async Task<Sonuc<DosyaBelgeleriVM>> DosyaBelgeleriVMGetirAsync(
        DosyaBelgeleriVM vm)
    {
        if (vm.Id < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {vm.Id} bulunamadı."
            };

        var q = _vt.DosyaBelgeleri
            .Where(db => db.DosyaId == vm.Id)
            .Select(db => new DosyaBelgeleriVM.Oge
            {
                Id = db.Id,
                Aciklama = db.Aciklama,
                Baslik = db.Baslik,
                Boyut = Yardimci.OkunabilirDosyaBoyutu(db.Boyut),
                OlusturmaTarihi = db.OlusturmaTarihi,
                Url = db.Url,
                Uzanti = db.Uzanti
            });

        if (!string.IsNullOrWhiteSpace(vm.Arama))
            q = q.Where(db =>
                (db.Aciklama != null && db.Aciklama.Contains(vm.Arama)) ||
                db.Baslik.Contains(vm.Arama) ||
                db.Boyut.Contains(vm.Arama) ||
                db.Uzanti.Contains(vm.Arama));

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Sayfa",
                HataMesaji = $"Sayfa: {vm.Sayfa}, Sayfa Boyutu: {vm.SayfaBoyutu}"
            };

        vm.ToplamSayfa = await q.ToplamSayfaAsync(vm.SayfaBoyutu);
        vm.Ogeler = await q.SayfaUygula(vm.Sayfa, vm.SayfaBoyutu).ToListAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<DosyaBelgesiEkleVM>> BelgeEkleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Dosyalar.AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = new DosyaBelgesiEkleVM { Id = id };

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> BelgeEkleAsync(
        DosyaBelgesiEkleVM vm, IFormFile? belge)
    {
        if (vm.Id < 1 || !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı"
            };

        var belgeAraci = new BelgeAraci
        {
            Belge = belge,
            Klasor = "belge",
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

        var model = new DosyaBelgesi
        {
            Baslik = vm.Baslik,
            Aciklama = vm.Aciklama,
            DosyaId = vm.Id,
            OlusturmaTarihi = DateTime.Now,
            Url = belgeAraci.Url!,
            Uzanti = belgeAraci.Uzanti!,
            Boyut = belgeAraci.Boyut
        };

        await _vt.DosyaBelgeleri.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = vm.Id };
    }

    public async Task<Sonuc<DosyaBelgesiDuzenleVM>> BelgeDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.DosyaBelgeleri.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.DosyaBelgeleri
            .Where(db => db.Id == id)
            .Select(db => new DosyaBelgesiDuzenleVM
            {
                Id = db.Id,
                Aciklama = db.Aciklama,
                Baslik = db.Baslik,
                DosyaId = db.DosyaId
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> BelgeDuzenleAsync(
        DosyaBelgesiDuzenleVM vm, IFormFile? belge)
    {
        var model = await _vt.DosyaBelgeleri.FirstOrDefaultAsync(db => db.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı"
            };

        if (vm.BelgeyiDegistir)
        {
            var yeniBelge = new BelgeAraci
            {
                Belge = belge,
                Root = _env.WebRootPath,
                Klasor = "belge",
                UstuneYaz = true
            };

            var sonuc = yeniBelge.Onayla();

            if (!sonuc.BasariliMi)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = sonuc.HataMesaji
                };

            var eskiBelge = new BelgeAraci
            {
                Yol = Path.Combine(_env.WebRootPath, model.Url[1..])
            };

            sonuc = eskiBelge.Sil();

            if (!sonuc.BasariliMi)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = sonuc.HataMesaji
                };

            sonuc = yeniBelge.Olustur();

            if (!sonuc.BasariliMi)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = sonuc.HataMesaji
                };

            model.Url = yeniBelge.Url!;
            model.Uzanti = yeniBelge.Uzanti!;
            model.Boyut = yeniBelge.Boyut;
        }

        model.Baslik = vm.Baslik;
        model.Aciklama = vm.Aciklama;

        _vt.DosyaBelgeleri.Update(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = model.DosyaId };
    }

    public async Task<Sonuc<DosyaBelgesiSilVM>> BelgeSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.DosyaBelgeleri.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.DosyaBelgeleri
            .Where(db => db.Id == id)
            .Select(db => new DosyaBelgesiSilVM
            {
                Id = db.Id,
                Aciklama = db.Aciklama,
                Baslik = db.Baslik,
                DosyaId = db.DosyaId,
                Boyut = Yardimci.OkunabilirDosyaBoyutu(db.Boyut),
                OlusturmaTarihi = db.OlusturmaTarihi,
                Url = db.Url,
                Uzanti = db.Uzanti
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> BelgeSilAsync(int id)
    {
        var model = await _vt.DosyaBelgeleri.FirstOrDefaultAsync(db => db.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı"
            };

            var belge = new BelgeAraci
            {
                Yol = Path.Combine(_env.WebRootPath, model.Url[1..])
            };

            var sonuc = belge.Sil();

            if (!sonuc.BasariliMi)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = sonuc.HataMesaji
                };

        var dosyaId = model.DosyaId;

        _vt.DosyaBelgeleri.Remove(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }
    #endregion

    #region Temizle
    public async Task PersonelBaglantlariniTemizleAsync(int id)
    {
        var modeller = await _vt.DosyaPersonel
            .Where(dp => dp.DosyaId == id)
            .ToListAsync();

        _vt.DosyaPersonel.RemoveRange(modeller);
        await _vt.SaveChangesAsync();
    }
    
    public async Task DosyaBaglantilariniTemizleAsync(int id)
    {
        var modeller = await _vt.DosyaBaglantilari
            .Where(db => db.IlgiliDosyaId == id)
            .ToListAsync();

        _vt.DosyaBaglantilari.RemoveRange(modeller);
        await _vt.SaveChangesAsync();
    }
    
    public async Task GorevBaglantilariniTemizleAsync(int id)
    {
        var modeller = await _vt.Gorevler
            .Where(db => db.DosyaId == id)
            .ToListAsync();

        _vt.Gorevler.RemoveRange(modeller);
        await _vt.SaveChangesAsync();
    }
    
    public async Task BelgeleriTemizleAsync(int id)
    {
        var modeller = await _vt.DosyaBelgeleri
            .Where(db => db.DosyaId == id)
            .ToListAsync();

        foreach (var model in modeller)
        {
            var belgeYolu = Path.Combine(_env.WebRootPath, model.Url[1..]);

            if (File.Exists(belgeYolu))
                File.Delete(belgeYolu);
        }

        _vt.DosyaBelgeleri.RemoveRange(modeller);
        await _vt.SaveChangesAsync();
    }

    public async Task FinansBaglantilariniTemizleAsync(int id)
    {
        var modeller = await _vt.FinansIslemleri
            .Where(f => f.DosyaId == id)
            .ToListAsync();

        foreach (var model in modeller)
        {
            model.DosyaBaglantisiVar = false;
            model.DosyaId = null;
        }

        _vt.FinansIslemleri.UpdateRange(modeller);
        await _vt.SaveChangesAsync();
    }

    public async Task KararBilgileriniTemizleAsync(int id)
    {
        var kararModel = await _vt.KararBilgileri.FirstAsync(k => k.DosyaId == id);
        var bolgeAdliyeModel = await _vt.BolgeAdliyeMahkemesiBilgileri.FirstAsync(k => k.DosyaId == id);
        var temyizModel = await _vt.TemyizBilgileri.FirstAsync(k => k.DosyaId == id);
        var duzeltmeModel = await _vt.KararDuzeltmeBilgileri.FirstAsync(k => k.DosyaId == id);
        var kesinlesmeModel = await _vt.KesinlesmeBilgileri.FirstAsync(k => k.DosyaId == id);

        _vt.KararBilgileri.Remove(kararModel);
        _vt.BolgeAdliyeMahkemesiBilgileri.Remove(bolgeAdliyeModel);
        _vt.TemyizBilgileri.Remove(temyizModel);
        _vt.KararDuzeltmeBilgileri.Remove(duzeltmeModel);
        _vt.KesinlesmeBilgileri.Remove(kesinlesmeModel);

        await _vt.SaveChangesAsync();
    }
    #endregion
}
