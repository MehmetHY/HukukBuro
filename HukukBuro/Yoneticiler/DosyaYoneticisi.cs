using HukukBuro.Araclar;
using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Karsilastiricilar;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Dosyalar;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class DosyaYoneticisi
{
    #region Fields
    private readonly VeriTabani _veritabani;
    private readonly IWebHostEnvironment _env;

    public DosyaYoneticisi(VeriTabani vt, IWebHostEnvironment env)
    {
        _veritabani = vt;
        _env = env;
    }
    #endregion

    #region Dosya
    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _veritabani.Dosyalar
            .AsNoTracking()
            .Include(d => d.DosyaTuru)
            .Include(d => d.DosyaKategorisi)
            .Include(d => d.DosyaDurumu)
            .Include(d => d.IlgiliGorevler)
            .Include(d => d.Durusmalar)
            .Include(d => d.Belgeler)
            .Where(d =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                d.DosyaNo.Contains(vm.Arama) ||
                d.BuroNo.Contains(vm.Arama) ||
                d.Konu.Contains(vm.Arama) ||
                d.DosyaTuru.Isim.Contains(vm.Arama) ||
                d.DosyaKategorisi.Isim.Contains(vm.Arama) ||
                d.DosyaDurumu.Isim.Contains(vm.Arama))
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
                BelgeSayisi = d.Belgeler.Count
            });

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
        => await _veritabani.DosyaTurleri
            .AsNoTracking()
            .Select(dt => new SelectListItem
            {
                Value = dt.Id.ToString(),
                Text = dt.Isim
            })
            .ToListAsync();

    public async Task<List<SelectListItem>> DosyaKategorileriGetirAsync()
        => await _veritabani.DosyaKategorileri
            .AsNoTracking()
            .Select(dk => new SelectListItem
            {
                Value = dk.Id.ToString(),
                Text = dk.Isim
            })
            .ToListAsync();

    public async Task<List<SelectListItem>> DosyaDurumlariGetirAsync()
        => await _veritabani.DosyaDurumu
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
        model.OlusturmaTarihi = DateTime.Now;

        await _veritabani.Dosyalar.AddAsync(model);
        await _veritabani.SaveChangesAsync();
    }

    public async Task<Sonuc<OzetVM>> OzetVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _veritabani.Dosyalar
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
                AcilisTarihi = d.AcilisTarihi,
                OlusturmaTarihi = d.OlusturmaTarihi
            })
            .FirstAsync();

        vm.Taraflar = await TarafGetirAsync(id);
        vm.SorumluPersonel = await OzetPersonelGetirAsync(id);
        vm.DosyaBaglantilari = await OzetDosyaBaglantilariGetirAsync(id);
        vm.Durusmalar = await OzetDurusmalariGetirAsync(id);
        vm.Belgeler = await OzetBelgeleriGetirAsync(id);

        return new() { Deger = vm };
    }

    public async Task<Sonuc<DuzenleVM>> DuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _veritabani.Dosyalar
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
        var model = await _veritabani.Dosyalar.FirstOrDefaultAsync(d => d.Id == vm.Id);

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

        _veritabani.Dosyalar.Update(model);
        await _veritabani.SaveChangesAsync();

        return new();
    }
    
    public async Task<Sonuc<SilVM>> SilVMGetirAsync(int id)
    {
        var vm = await _veritabani.Dosyalar
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
                DosyaTuru = d.DosyaTuru.Isim,
                Mahkeme = d.Mahkeme,
                AcilisTarihi = d.AcilisTarihi,
                OlusturmaTarihi = d.OlusturmaTarihi
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
        var model = await _veritabani.Dosyalar.FirstOrDefaultAsync(d => d.Id == id);

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

        _veritabani.Dosyalar.Remove(model);
        await _veritabani.SaveChangesAsync();

        return new();
    }
    #endregion

    #region Taraf
    public async Task<Sonuc<TarafEkleVM>> TarafEkleVMGetirAsync(int dosyaId)
    {
        if (dosyaId < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == dosyaId))
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
        if (vm.DosyaId < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
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

        await _veritabani.TarafKisiler.AddAsync(model);
        await _veritabani.SaveChangesAsync();

        return new();
    }

    public async Task<List<SelectListItem>> TarafTurleriGetirAsync()
        => await _veritabani.TarafTurleri
        .AsNoTracking()
        .Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Isim
        })
        .ToListAsync();

    public async Task<List<SelectListItem>> KisileriGetirAsync()
        => await _veritabani.Kisiler
        .AsNoTracking()
        .Select(k => new SelectListItem
        {
            Value = k.Id.ToString(),
            Text = k.TuzelMi ? k.SirketIsmi! : $"{k.Isim} {k.Soyisim}"
        })
        .ToListAsync();

    public async Task<List<OzetVM.Taraf>> TarafGetirAsync(
        int dosyaId)
        => await _veritabani.TarafKisiler
            .AsNoTracking()
            .Where(t => t.DosyaId == dosyaId)
            .Include(t => t.Kisi)
            .Include(t => t.TarafTuru)
            .Select(t => new OzetVM.Taraf
            {
                Id = t.Id,

                Isim = t.Kisi.TuzelMi ?
                    t.Kisi.SirketIsmi! :
                    $"{t.Kisi.Isim} {t.Kisi.Soyisim}",

                TarafTuru = t.TarafTuru.Isim,
                KarsiTaraf = t.KarsiTaraf,
                KisiId = t.KisiId
            })
            .ToListAsync();

    public async Task<Sonuc<TarafDuzenleVM>> TarafDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.TarafKisiler.AnyAsync(t => t.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _veritabani.TarafKisiler
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
        var model = await _veritabani.TarafKisiler.FirstOrDefaultAsync(t => t.Id == vm.Id);

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

        _veritabani.TarafKisiler.Update(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = model.DosyaId };
    }

    public async Task<Sonuc<TarafSilVM>> TarafSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.TarafKisiler.AnyAsync(t => t.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _veritabani.TarafKisiler
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
        var model = await _veritabani.TarafKisiler.FirstOrDefaultAsync(t => t.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var dosyaId = model.DosyaId;
        _veritabani.TarafKisiler.Remove(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }
    #endregion

    #region Personel
    public async Task<Sonuc<PersonelDuzenleVM>> PersonelDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var vm = new PersonelDuzenleVM { Id = id };

        vm.PersonelListe = await _veritabani.Users
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
        if (vm.Id < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz id",
                HataMesaji = $"Id: {vm.Id} bulunamadı."
            };

        var personeller = await _veritabani.Users
            .Include(p => p.SorumluDosyalar)
            .ToListAsync();

        foreach (var personel in personeller)
        {
            var item = vm.PersonelListe.FirstOrDefault(p => p.Value == personel.Id);

            if (item == null)
                continue;

            var dosyaPersonel = await _veritabani.DosyaPersonel
                .FirstOrDefaultAsync(dp =>
                    dp.DosyaId == vm.Id &&
                    dp.PersonelId == item.Value);

            if (dosyaPersonel != null && !item.Checked)
                _veritabani.DosyaPersonel.Remove(dosyaPersonel);
            else if (dosyaPersonel == null && item.Checked)
                await _veritabani.DosyaPersonel.AddAsync(new()
                {
                    DosyaId = vm.Id,
                    PersonelId = item.Value
                });
        }

        await _veritabani.SaveChangesAsync();

        return new() { Deger = vm.Id };
    }

    public async Task<List<OzetVM.Personel>> OzetPersonelGetirAsync(int id)
    {
        var personel = await _veritabani.DosyaPersonel
            .Where(dp => dp.DosyaId == id)
            .Select(dp => new OzetVM.Personel
            {
                Id = dp.PersonelId,
                TamIsim = $"{dp.Personel.Isim} {dp.Personel.Soyisim}",

                AnaRol = _veritabani.UserClaims
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
            !await _veritabani.Dosyalar.AnyAsync(d => d.Id == dosyaId))
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
            !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.DosyaId} bulunamadı."
            };

        if (vm.IlgiliDosyaId < 1 ||
            !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.IlgiliDosyaId))
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

        await _veritabani.DosyaBaglantilari.AddAsync(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }

    public async Task<List<SelectListItem>> DosyalariGetirAsync()
        => await _veritabani.Dosyalar
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.DosyaNo} {d.BuroNo} {d.Konu}"
            })
            .ToListAsync();

    public async Task<List<OzetVM.Baglanti>> OzetDosyaBaglantilariGetirAsync(int id)
        => await _veritabani.DosyaBaglantilari
        .Include(db => db.IlgiliDosya)
        .ThenInclude(d => d.DosyaTuru)
        .Include(db => db.IlgiliDosya)
        .ThenInclude(d => d.DosyaKategorisi)
        .Include(db => db.IlgiliDosya)
        .ThenInclude(d => d.DosyaDurumu)
        .Where(db => db.DosyaId == id)
        .Select(db => new OzetVM.Baglanti
        {
            Id = db.Id,
            IlgiliDosyaId = db.IlgiliDosyaId,
            Dosya = db.IlgiliDosya.TamIsim,
            Tur = db.IlgiliDosya.DosyaTuru.Isim,
            Kategori = db.IlgiliDosya.DosyaKategorisi.Isim,
            Durum = db.IlgiliDosya.DosyaDurumu.Isim,
            Aciklama = db.Aciklama
        })
        .ToListAsync();

    public async Task<Sonuc<DosyaBaglantisiDuzenleVM>>
        DosyaBaglantisiDuzenleVMGetirAsync(int id)
    {
        if (id < 1 ||
            !await _veritabani.DosyaBaglantilari.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _veritabani.DosyaBaglantilari
            .Where(db => db.Id == id)
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
        var model = await _veritabani.DosyaBaglantilari
            .FirstOrDefaultAsync(db => db.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı."
            };

        if (vm.DosyaId < 1 ||
            !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.DosyaId),
                HataMesaji = $"id: {vm.DosyaId} bulunamadı."
            };

        if (vm.IlgiliDosyaId < 1 ||
            !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.IlgiliDosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.IlgiliDosyaId),
                HataMesaji = $"id: {vm.IlgiliDosyaId} bulunamadı."
            };

        model.Aciklama = vm.Aciklama;
        model.DosyaId = vm.DosyaId;
        model.IlgiliDosyaId = vm.IlgiliDosyaId;
        _veritabani.DosyaBaglantilari.Update(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }

    public async Task<Sonuc<DosyaBaglantisiSilVM>> DosyaBaglantisiSilVMGetirAsync(
        int id)
    {
        if (id < 1 ||
            !await _veritabani.DosyaBaglantilari.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var vm = await _veritabani.DosyaBaglantilari
            .Include(db => db.IlgiliDosya)
            .Where(db => db.Id == id)
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
        var model = await _veritabani.DosyaBaglantilari
            .FirstOrDefaultAsync(db => db.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        var dosyaId = model.DosyaId;
        _veritabani.DosyaBaglantilari.Remove(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }
    #endregion

    #region Karar
    public async Task<Sonuc<KararVM>> KararVMGetirAsync(int dosyaId)
    {
        if (dosyaId < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == dosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {dosyaId} bulunamadı."
            };

        var vm = await _veritabani.Dosyalar
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
        var dosya = await _veritabani.Dosyalar
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

        _veritabani.KararBilgileri.Update(dosya.KararBilgileri);
        _veritabani.BolgeAdliyeMahkemesiBilgileri.Update(dosya.BolgeAdliyeMahkemesiBilgileri);
        _veritabani.TemyizBilgileri.Update(dosya.TemyizBilgileri);
        _veritabani.KararDuzeltmeBilgileri.Update(dosya.KararDuzeltmeBilgileri);
        _veritabani.KesinlesmeBilgileri.Update(dosya.KesinlesmeBilgileri);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }
    #endregion

    #region Durusma
    public async Task<List<OzetVM.Durusma>> OzetDurusmalariGetirAsync(int id)
        => await _veritabani.Durusmalar
        .Where(d => d.DosyaId == id)
        .Include(d => d.AktiviteTuru)
        .OrderBy(d => d.Tamamlandi)
        .ThenByDescending(d => d.Tarih)
        .Select(d => new OzetVM.Durusma
        {
            Id = d.Id,
            AktiviteTuru = d.AktiviteTuru.Isim,
            Tarih = d.Tarih,
            Aciklama = d.Aciklama,
            Tamamlandi = d.Tamamlandi
        })
        .ToListAsync();

    public async Task<Sonuc<DurusmaEkleVM>> DurusmaEkleVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == id))
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
        if (vm.DosyaId < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
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
            Tarih = vm.Tarih,
            Tamamlandi = vm.Tamamlandi
        };

        await _veritabani.Durusmalar.AddAsync(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = vm.DosyaId };
    }
    public async Task<Sonuc<DurusmaDuzenleVM>> DurusmaDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.Durusmalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var vm = await _veritabani.Durusmalar
            .Where(d => d.Id == id)
            .Select(d => new DurusmaDuzenleVM
            {
                Id = d.Id,
                DosyaId = d.DosyaId,
                Aciklama = d.Aciklama,
                AktiviteTuruId = d.AktiviteTuruId,
                Tarih = d.Tarih,
                Tamamlandi = d.Tamamlandi
            })
            .FirstAsync();

        vm.AktiviteTurleri = await DurusmaAktiviteTurleriGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DurusmaDuzenleAsync(DurusmaDuzenleVM vm)
    {
        var model = await _veritabani.Durusmalar.FirstOrDefaultAsync(d => d.Id == vm.Id);

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
        model.Tamamlandi = vm.Tamamlandi;

        _veritabani.Durusmalar.Update(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = model.DosyaId };
    }

    public async Task<Sonuc<DurusmaSilVM>> DurusmaSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.Durusmalar.AnyAsync(d => d.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var vm = await _veritabani.Durusmalar
            .Where(d => d.Id == id)
            .Include(d => d.AktiviteTuru)
            .Select(d => new DurusmaSilVM
            {
                Id = d.Id,
                DosyaId = d.DosyaId,
                Aciklama = d.Aciklama,
                AktiviteTuru = d.AktiviteTuru.Isim,
                Tarih = d.Tarih,
                Tamamlandi = d.Tamamlandi
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> DurusmaSilAsync(int id)
    {
        var model = await _veritabani.Durusmalar.FirstOrDefaultAsync(d => d.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {id} bulunamadı."
            };

        var dosyaId = model.DosyaId;

        _veritabani.Durusmalar.Remove(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }

    public async Task<List<SelectListItem>> DurusmaAktiviteTurleriGetirAsync()
        => await _veritabani.DurusmaAktiviteTurleri
        .Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Isim
        })
        .ToListAsync();
    
    public async Task<Sonuc<DurusmalarVM>> DurusmalarVMGetirAsync(DurusmalarVM vm)
    {
        var q = _veritabani.Durusmalar
            .Include(d => d.Dosya)
            .Include(d => d.AktiviteTuru)
            .Where(d =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                (d.Aciklama != null && d.Aciklama.Contains(vm.Arama)) ||
                d.Dosya.Konu.Contains(vm.Arama) ||
                d.Dosya.BuroNo.Contains(vm.Arama) ||
                d.Dosya.DosyaNo.Contains(vm.Arama) ||
                d.AktiviteTuru.Isim.Contains(vm.Arama) ||
                d.Tarih.ToString().Contains(vm.Arama))
            .OrderBy(d => d.Tamamlandi)
            .ThenBy(d => d.Tarih)
            .Select(d => new DurusmaVM
            {
                Id = d.Id,
                DosyaId = d.DosyaId,
                Aciklama = d.Aciklama,
                AktiviteTuru = d.AktiviteTuru.Isim,
                DosyaTamisim = d.Dosya.TamIsim,
                Tarih = d.Tarih,
                Tamamlandi = d.Tamamlandi
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
    #endregion

    #region DosyaBelgesi
    public async Task<Sonuc<DosyaBelgeleriVM>> DosyaBelgeleriVMGetirAsync(
        DosyaBelgeleriVM vm)
    {
        if (vm.Id < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"Id: {vm.Id} bulunamadı."
            };

        var q = _veritabani.DosyaBelgeleri
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
        if (id < 1 || !await _veritabani.Dosyalar.AnyAsync(k => k.Id == id))
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
        if (vm.Id < 1 || !await _veritabani.Dosyalar.AnyAsync(d => d.Id == vm.Id))
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

        await _veritabani.DosyaBelgeleri.AddAsync(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = vm.Id };
    }

    public async Task<Sonuc<DosyaBelgesiDuzenleVM>> BelgeDuzenleVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.DosyaBelgeleri.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _veritabani.DosyaBelgeleri
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
        var model = await _veritabani.DosyaBelgeleri.FirstOrDefaultAsync(db => db.Id == vm.Id);

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

        _veritabani.DosyaBelgeleri.Update(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = model.DosyaId };
    }

    public async Task<Sonuc<DosyaBelgesiSilVM>> BelgeSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _veritabani.DosyaBelgeleri.AnyAsync(db => db.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _veritabani.DosyaBelgeleri
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
        var model = await _veritabani.DosyaBelgeleri.FirstOrDefaultAsync(db => db.Id == id);

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

        _veritabani.DosyaBelgeleri.Remove(model);
        await _veritabani.SaveChangesAsync();

        return new() { Deger = dosyaId };
    }

    public async Task<List<OzetVM.Belge>> OzetBelgeleriGetirAsync(int id)
        => await _veritabani.DosyaBelgeleri
        .Where(db => db.DosyaId == id)
        .Select(db => new OzetVM.Belge
        {
            Id = db.Id,
            Baslik = db.Baslik,
            Aciklama = db.Aciklama,
            Boyut = Yardimci.OkunabilirDosyaBoyutu(db.Boyut),
            Tarih = db.OlusturmaTarihi,
            Url = db.Url,
            Uzanti = db.Uzanti
        })
        .ToListAsync();
    #endregion

    #region Temizle
    public async Task PersonelBaglantlariniTemizleAsync(int id)
    {
        var modeller = await _veritabani.DosyaPersonel
            .Where(dp => dp.DosyaId == id)
            .ToListAsync();

        _veritabani.DosyaPersonel.RemoveRange(modeller);
        await _veritabani.SaveChangesAsync();
    }
    
    public async Task DosyaBaglantilariniTemizleAsync(int id)
    {
        var modeller = await _veritabani.DosyaBaglantilari
            .Where(db => db.IlgiliDosyaId == id)
            .ToListAsync();

        _veritabani.DosyaBaglantilari.RemoveRange(modeller);
        await _veritabani.SaveChangesAsync();
    }
    
    public async Task GorevBaglantilariniTemizleAsync(int id)
    {
        var modeller = await _veritabani.Gorevler
            .Where(db => db.DosyaId == id)
            .ToListAsync();

        _veritabani.Gorevler.RemoveRange(modeller);
        await _veritabani.SaveChangesAsync();
    }
    
    public async Task BelgeleriTemizleAsync(int id)
    {
        var modeller = await _veritabani.DosyaBelgeleri
            .Where(db => db.DosyaId == id)
            .ToListAsync();

        foreach (var model in modeller)
        {
            var belgeYolu = Path.Combine(_env.WebRootPath, model.Url[1..]);

            if (File.Exists(belgeYolu))
                File.Delete(belgeYolu);
        }

        _veritabani.DosyaBelgeleri.RemoveRange(modeller);
        await _veritabani.SaveChangesAsync();
    }

    public async Task FinansBaglantilariniTemizleAsync(int id)
    {
        var modeller = await _veritabani.FinansIslemleri
            .Where(f => f.DosyaId == id)
            .ToListAsync();

        foreach (var model in modeller)
        {
            model.DosyaBaglantisiVar = false;
            model.DosyaId = null;
        }

        _veritabani.FinansIslemleri.UpdateRange(modeller);
        await _veritabani.SaveChangesAsync();
    }

    public async Task KararBilgileriniTemizleAsync(int id)
    {
        var kararModel = await _veritabani.KararBilgileri.FirstAsync(k => k.DosyaId == id);
        var bolgeAdliyeModel = await _veritabani.BolgeAdliyeMahkemesiBilgileri.FirstAsync(k => k.DosyaId == id);
        var temyizModel = await _veritabani.TemyizBilgileri.FirstAsync(k => k.DosyaId == id);
        var duzeltmeModel = await _veritabani.KararDuzeltmeBilgileri.FirstAsync(k => k.DosyaId == id);
        var kesinlesmeModel = await _veritabani.KesinlesmeBilgileri.FirstAsync(k => k.DosyaId == id);

        _veritabani.KararBilgileri.Remove(kararModel);
        _veritabani.BolgeAdliyeMahkemesiBilgileri.Remove(bolgeAdliyeModel);
        _veritabani.TemyizBilgileri.Remove(temyizModel);
        _veritabani.KararDuzeltmeBilgileri.Remove(duzeltmeModel);
        _veritabani.KesinlesmeBilgileri.Remove(kesinlesmeModel);

        await _veritabani.SaveChangesAsync();
    }
    #endregion
}
