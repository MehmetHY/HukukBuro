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

        vm.ToplamSayfa = await q.SayfaSayisi(vm.SayfaBoyutu);

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
    #endregion
}
