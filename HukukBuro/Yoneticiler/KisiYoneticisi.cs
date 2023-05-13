using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Kisiler;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class KisiYoneticisi
{
    #region Fields
    private readonly VeriTabani _vt;
    private readonly IWebHostEnvironment _env;

    public KisiYoneticisi(VeriTabani vt, IWebHostEnvironment env)
    {
        _vt = vt;
        _env = env;
    }
    #endregion

    #region Kisi
    public async Task<Sonuc> EkleAsync(KisilerEkleVM vm)
    {
        return vm.TuzelMi ? await SirketEkleAsync(vm) : await KisiEkleAsync(vm);
    }

    private async Task<Sonuc> KisiEkleAsync(KisilerEkleVM vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Isim))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.Isim),
                HataMesaji = "İsim gerekli"
            };

        if (string.IsNullOrWhiteSpace(vm.Soyisim))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.Soyisim),
                HataMesaji = "Soyisim gerekli"
            };

        vm.Isim = vm.Isim.Trim();
        vm.Soyisim = vm.Soyisim.Trim();

        var model = new Kisi
        {
            TuzelMi = false,
            Kisaltma = $"{vm.Isim[0]}{vm.Soyisim[0]}".ToUpper(),
            Isim = vm.Isim,
            Soyisim = vm.Soyisim,
            KimlikNo = vm.TcKimlikNo?.Trim(),
            Telefon = vm.Telefon?.Trim(),
            Email = vm.Email?.Trim(),
            AdresBilgisi = vm.Adres?.Trim(),
            BankaHesapBilgisi = vm.BankaHesapBilgisi?.Trim(),
            EkBilgi = vm.EkBilgi?.Trim()
        };

        await _vt.Kisiler.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    private async Task<Sonuc> SirketEkleAsync(KisilerEkleVM vm)
    {
        if (string.IsNullOrWhiteSpace(vm.SirketIsmi))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.SirketIsmi),
                HataMesaji = "Şirket ismi gerekli"
            };

        vm.SirketIsmi = vm.SirketIsmi.Trim();

        var model = new Kisi
        {
            TuzelMi = true,
            Kisaltma = vm.SirketIsmi.Length == 1 ?
                vm.SirketIsmi[0].ToString().ToUpper() :
                vm.SirketIsmi[..2].ToUpper(),

            SirketIsmi = vm.SirketIsmi,
            VergiDairesi = vm.VergiDairesi?.Trim(),
            VergiNo = vm.VergiNo?.Trim(),
            Telefon = vm.Telefon?.Trim(),
            Email = vm.Email?.Trim(),
            AdresBilgisi = vm.Adres?.Trim(),
            BankaHesapBilgisi = vm.BankaHesapBilgisi?.Trim(),
            EkBilgi = vm.EkBilgi?.Trim()
        };

        await _vt.Kisiler.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<KisilerListeleVM>> ListeleVMGetirAsync(
        string arama, int sayfa, int sayfaBoyutu
    )
    {
        var sonuc = new Sonuc<KisilerListeleVM>();
        var q = _vt.Kisiler.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(arama))
            q = q.Where(k =>
                (k.Kisaltma != null && k.Kisaltma.Contains(arama)) ||
                (k.Isim != null && k.Isim.Contains(arama)) ||
                (k.Soyisim != null && k.Soyisim.Contains(arama)) ||
                (k.KimlikNo != null && k.KimlikNo.Contains(arama)) ||
                (k.SirketIsmi != null && k.SirketIsmi.Contains(arama)) ||
                (k.VergiDairesi != null && k.VergiDairesi.Contains(arama)) ||
                (k.VergiNo != null && k.VergiNo.Contains(arama)) ||
                (k.Telefon != null && k.Telefon.Contains(arama)) ||
                (k.Email != null && k.Email.Contains(arama)) ||
                (k.AdresBilgisi != null && k.AdresBilgisi.Contains(arama)) ||
                (k.BankaHesapBilgisi != null && k.BankaHesapBilgisi.Contains(arama)) ||
                (k.EkBilgi != null && k.EkBilgi.Contains(arama))
            );

        if (!await q.SayfaGecerliMiAsync(sayfa, sayfaBoyutu))
        {
            sonuc.BasariliMi = false;
            sonuc.HataBasligi = "Geçersiz Sayfa";
            sonuc.HataMesaji = $"sayfa: {sayfa}, sayfa boyutu: {sayfaBoyutu}";

            return sonuc;
        }

        sonuc.Deger = new();
        sonuc.Deger.ToplamSayfa = await q.ToplamSayfaAsync(sayfaBoyutu);
        sonuc.Deger.Sayfa = sayfa;
        sonuc.Deger.Arama = arama;
        sonuc.Deger.SayfaBoyutu = sayfaBoyutu;

        sonuc.Deger.Ogeler = await q.SayfaUygula(sayfa, sayfaBoyutu)
            .Include(k => k.IlgiliDosyalar)
            .Include(k => k.Randevular)
            .Include(k => k.IlgiliGorevler).ThenInclude(g => g.Durum)
            .Include(k => k.IlgiliFinansIslemleri)
            .OrderBy(k => k.Kisaltma)
            .Select(k => new KisilerListeleVM.Oge
            {
                Id = k.Id,
                Kisaltma = k.Kisaltma,
                TuzelMi = k.TuzelMi,

                Isim = k.Isim != null && k.Soyisim != null ?
                    $"{k.Isim} {k.Soyisim}" :
                    k.SirketIsmi ?? string.Empty,

                KimlikVergiNo = k.KimlikNo ?? k.VergiNo ?? string.Empty,
                Telefon = k.Telefon ?? string.Empty,
                Email = k.Email ?? string.Empty,
                DosyaSayisi = k.IlgiliDosyalar.Count,
                RandevuSayisi = k.Randevular.Where(r => r.TamamlandiMi).Count(),

                GorevSayisi = k.IlgiliGorevler.Where(
                    g => g.Durum.Isim == Sabit.GorevDurumu.DevamEdiyor).Count(),

                FinansSayisi = k.IlgiliFinansIslemleri.Where(f => !f.Odendi).Count()
            })
            .ToListAsync();

        return sonuc;
    }

    public async Task<Sonuc<KisiOzetVM>> OzetVMGetirAsync(int id)
    {
        if (id < 1)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.Kisiler
            .AsNoTracking()
            .Where(k => k.Id == id)
            .Include(k => k.IlgiliKisiler)
            .Include(k => k.IlgiliDosyalar)
            .Include(k => k.Randevular)
            .Include(k => k.IlgiliGorevler)
            .Include(k => k.IlgiliFinansIslemleri)
            .Include(k => k.Belgeler)
            .Select(k => new KisiOzetVM
            {
                Id = id,
                TuzelMi = k.TuzelMi,
                Isim = k.Isim,
                Soyisim = k.Soyisim,
                TcKimlikNo = k.KimlikNo,
                SirketIsmi = k.SirketIsmi,
                VergiDairesi = k.VergiDairesi,
                VergiNo = k.VergiNo,
                Telefon = k.Telefon,
                Email = k.Email,
                Adres = k.AdresBilgisi,
                BankaHesapBilgisi = k.BankaHesapBilgisi,
                EkBilgi = k.EkBilgi,

                IlgiliKisiSayisi = k.IlgiliKisiler.Count(),
                IlgiliDosyaSayisi = k.IlgiliDosyalar.Count(),
                RandevuSayisi = k.Randevular.Count(),
                GorevSayisi = k.IlgiliGorevler.Count(),
                FinansSayisi = k.IlgiliFinansIslemleri.Count(),
                BelgeSayisi = k.Belgeler.Count()
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        return new() { Deger = vm };
    }

    public async Task<Sonuc<KisiSilVM>> SilVMGetirAsync(int id)
    {
        if (id < 1)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.Kisiler
            .AsNoTracking()
            .Where(k => k.Id == id)
            .Include(k => k.IlgiliKisiler)
            .Include(k => k.IlgiliDosyalar)
            .Include(k => k.Randevular)
            .Include(k => k.IlgiliGorevler)
            .Include(k => k.IlgiliFinansIslemleri)
            .Include(k => k.Belgeler)
            .Select(k => new KisiSilVM
            {
                Id = id,
                TuzelMi = k.TuzelMi,
                Isim = k.Isim,
                Soyisim = k.Soyisim,
                SirketIsmi = k.SirketIsmi,

                IlgiliKisiSayisi = k.IlgiliKisiler.Count(),
                IlgiliDosyaSayisi = k.IlgiliDosyalar.Count(),
                RandevuSayisi = k.Randevular.Count(),
                GorevSayisi = k.IlgiliGorevler.Count(),
                FinansSayisi = k.IlgiliFinansIslemleri.Count(),
                BelgeSayisi = k.Belgeler.Count()
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        return new() { Deger = vm };
    }

    public async Task<Sonuc> SilAsync(int id)
    {
        if (id < 1)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var model = await _vt.Kisiler.FirstOrDefaultAsync(k => k.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        await KisiBaglantilarindakiKayitlariSilAsync(id);
        _vt.Kisiler.Remove(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<KisiOzetDuzenleVM>> OzetDuzenleVMGetirAsync(int id)
    {
        if (id < 1)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.Kisiler
            .AsNoTracking()
            .Where(k => k.Id == id)
            .Include(k => k.IlgiliKisiler)
            .Include(k => k.IlgiliDosyalar)
            .Include(k => k.Randevular)
            .Include(k => k.IlgiliGorevler)
            .Include(k => k.IlgiliFinansIslemleri)
            .Include(k => k.Belgeler)
            .Select(k => new KisiOzetDuzenleVM
            {
                Id = id,
                TuzelMi = k.TuzelMi,
                Isim = k.Isim,
                Soyisim = k.Soyisim,
                TcKimlikNo = k.KimlikNo,
                SirketIsmi = k.SirketIsmi,
                VergiDairesi = k.VergiDairesi,
                VergiNo = k.VergiNo,
                Telefon = k.Telefon,
                Email = k.Email,
                Adres = k.AdresBilgisi,
                BankaHesapBilgisi = k.BankaHesapBilgisi,
                EkBilgi = k.EkBilgi,
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        return new() { Deger = vm };
    }

    public async Task<Sonuc> OzetDuzenleAsync(KisiOzetDuzenleVM vm)
    {
        var model = await _vt.Kisiler.FirstOrDefaultAsync(k => k.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {vm.Id} bulunamadı"
            };

        return vm.TuzelMi ?
            await SirketOzetDuzenleAsync(vm, model) :
            await KisiOzetDuzenleAsync(vm, model);
    }

    private async Task<Sonuc> KisiOzetDuzenleAsync(KisiOzetDuzenleVM vm, Kisi model)
    {
        if (string.IsNullOrWhiteSpace(vm.Isim))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.Isim),
                HataMesaji = "İsim gerekli"
            };

        if (string.IsNullOrWhiteSpace(vm.Soyisim))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.Soyisim),
                HataMesaji = "Soyisim gerekli"
            };

        vm.Isim = vm.Isim.Trim();
        vm.Soyisim = vm.Soyisim.Trim();

        model.TuzelMi = false;
        model.Kisaltma = $"{vm.Isim[0]}{vm.Soyisim[0]}".ToUpper();
        model.Isim = vm.Isim;
        model.Soyisim = vm.Soyisim;
        model.KimlikNo = vm.TcKimlikNo?.Trim();
        model.Telefon = vm.Telefon?.Trim();
        model.Email = vm.Email?.Trim();
        model.AdresBilgisi = vm.Adres?.Trim();
        model.BankaHesapBilgisi = vm.BankaHesapBilgisi?.Trim();
        model.EkBilgi = vm.EkBilgi?.Trim();

        model.SirketIsmi = null;
        model.VergiDairesi = null;
        model.VergiNo = null;

        _vt.Kisiler.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    private async Task<Sonuc> SirketOzetDuzenleAsync(KisiOzetDuzenleVM vm, Kisi model)
    {
        if (string.IsNullOrWhiteSpace(vm.SirketIsmi))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.SirketIsmi),
                HataMesaji = "Şirket ismi gerekli"
            };

        vm.SirketIsmi = vm.SirketIsmi.Trim();

        model.TuzelMi = true;
        model.Kisaltma = vm.SirketIsmi.Length == 1 ?
            vm.SirketIsmi[0].ToString().ToUpper() :
            vm.SirketIsmi[..2].ToUpper();

        model.SirketIsmi = vm.SirketIsmi;
        model.VergiDairesi = vm.VergiDairesi?.Trim();
        model.VergiNo = vm.VergiNo?.Trim();
        model.Telefon = vm.Telefon?.Trim();
        model.Email = vm.Email?.Trim();
        model.AdresBilgisi = vm.Adres?.Trim();
        model.BankaHesapBilgisi = vm.BankaHesapBilgisi?.Trim();
        model.EkBilgi = vm.EkBilgi?.Trim();

        model.Isim = null;
        model.Soyisim = null;
        model.KimlikNo = null;

        _vt.Kisiler.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }
    #endregion

    #region KisiBaglantisi
    public async Task<Sonuc<IlgiliKisilerVM>> IlgiliKisilerVMGetirAsync(
        int id,
        string arama,
        int sayfa,
        int sayfaBoyutu)
    {
        if (id < 1 || !await _vt.Kisiler.AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var q = _vt.KisiBaglantilari
            .Include(k => k.IlgiliKisi)
            .Where(k => k.KisiId == id && k.IlgiliKisi != null)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(arama))
            q = q.Where(k =>
                k.IlgiliKisi != null &&
                ((k.IlgiliKisi.Kisaltma != null && k.IlgiliKisi.Kisaltma.Contains(arama)) ||
                (k.IlgiliKisi.Isim != null && k.IlgiliKisi.Isim.Contains(arama)) ||
                (k.IlgiliKisi.Soyisim != null && k.IlgiliKisi.Soyisim.Contains(arama)) ||
                (k.IlgiliKisi.KimlikNo != null && k.IlgiliKisi.KimlikNo.Contains(arama)) ||
                (k.IlgiliKisi.SirketIsmi != null && k.IlgiliKisi.SirketIsmi.Contains(arama)) ||
                (k.IlgiliKisi.VergiDairesi != null && k.IlgiliKisi.VergiDairesi.Contains(arama)) ||
                (k.IlgiliKisi.VergiNo != null && k.IlgiliKisi.VergiNo.Contains(arama)) ||
                (k.IlgiliKisi.Telefon != null && k.IlgiliKisi.Telefon.Contains(arama)) ||
                (k.IlgiliKisi.Email != null && k.IlgiliKisi.Email.Contains(arama)) ||
                (k.IlgiliKisi.AdresBilgisi != null && k.IlgiliKisi.AdresBilgisi.Contains(arama)) ||
                (k.IlgiliKisi.BankaHesapBilgisi != null && k.IlgiliKisi.BankaHesapBilgisi.Contains(arama)) ||
                (k.IlgiliKisi.EkBilgi != null && k.IlgiliKisi.EkBilgi.Contains(arama)) ||
                (k.Pozisyon != null && k.Pozisyon.Contains(arama)))
            );

        if (!await q.SayfaGecerliMiAsync(sayfa, sayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Sayfa",
                HataMesaji = $"sayfa = {sayfa}, sayfa boyutu = {sayfaBoyutu}"
            };

        var deger = new IlgiliKisilerVM
        {
            Id = id,
            Sayfa = sayfa,
            ToplamSayfa = await q.ToplamSayfaAsync(sayfaBoyutu),
            SayfaBoyutu = sayfaBoyutu,
            Arama = arama
        };

        deger.Ogeler = await q
            .Select(k => new IlgiliKisilerVM.Oge
            {
                Id = k.Id,
                Isim = k.IlgiliKisi!.TuzelMi ?
                    k.IlgiliKisi.SirketIsmi! :
                    $"{k.IlgiliKisi.Isim} {k.IlgiliKisi.Soyisim}",

                Pozisyon = k.Pozisyon
            })
            .ToListAsync();

        return new() { Deger = deger };
    }

    public async Task<Sonuc<IlgiliKisiEkleVM>> IlgiliKisiEkleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Kisiler.AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = new IlgiliKisiEkleVM { KisiId = id };
        vm.Kisiler = await KisilerSelectListItemGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc> IlgiliKisiEkleAsync(IlgiliKisiEkleVM vm)
    {
        if (vm.KisiId < 1 ||
            vm.IlgiliKisiId < 1 ||
            !await _vt.Kisiler.AnyAsync(k => k.Id == vm.KisiId) ||
            !await _vt.Kisiler.AnyAsync(k => k.Id == vm.IlgiliKisiId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.KisiId} ya da {vm.IlgiliKisiId} bulunamadı"
            };


        var model = new KisiBaglantisi
        {
            KisiId = vm.KisiId,
            IlgiliKisiId = vm.IlgiliKisiId,
            Pozisyon = vm.Pozisyon
        };

        await _vt.KisiBaglantilari.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<List<SelectListItem>> KisilerSelectListItemGetirAsync(
        int id = 0)
        => await _vt.Kisiler
            .AsNoTracking()
            .Where(k => k.Id != id)
            .Select(k => new SelectListItem
            {
                Value = k.Id.ToString(),
                Text = k.TuzelMi ? k.SirketIsmi : $"{k.Isim} {k.Soyisim}"
            })
            .ToListAsync();

    public async Task<Sonuc<IlgiliKisiDuzenleVM>> IlgiliKisiDuzenleVMGetirAsync(int id)
    {
        if (id < 1 ||
            !await _vt.KisiBaglantilari
                .AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi Bağlantısı",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.KisiBaglantilari
            .AsNoTracking()
            .Where(k => k.Id == id)
            .Select(k => new IlgiliKisiDuzenleVM
            {
                Id = id,
                IlgiliKisiId = k.IlgiliKisiId,
                KisiId = k.KisiId,
                Pozisyon = k.Pozisyon
            })
            .FirstAsync();

        vm.Kisiler = await KisilerSelectListItemGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc> IlgiliKisiDuzenleAsync(IlgiliKisiDuzenleVM vm)
    {
        if (vm.Id < 1 ||
            !await _vt.KisiBaglantilari.AnyAsync(k => k.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"Geçersiz id: {vm.Id}"
            };

        if (vm.KisiId < 1 ||
            !await _vt.Kisiler.AnyAsync(k => k.Id == vm.KisiId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.KisiId),
                HataMesaji = "Geçersiz kişi"
            };

        if (vm.IlgiliKisiId < 1 ||
            !await _vt.Kisiler.AnyAsync(k => k.Id == vm.IlgiliKisiId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.IlgiliKisiId),
                HataMesaji = "Geçersiz kişi"
            };

        var model = await _vt.KisiBaglantilari.FirstAsync(k => k.Id == vm.Id);
        model.KisiId = vm.KisiId;
        model.IlgiliKisiId = vm.IlgiliKisiId;
        model.Pozisyon = vm.Pozisyon;
        _vt.KisiBaglantilari.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task KisiBaglantilarindakiKayitlariSilAsync(int id)
    {
        var modeller = await _vt.KisiBaglantilari
            .Where(kb => kb.IlgiliKisiId == id)
            .ToListAsync();

        _vt.KisiBaglantilari.RemoveRange(modeller);
        await _vt.SaveChangesAsync();
    }

    public async Task<Sonuc<IlgiliKisiSilVM>> IlgiliKisiSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.KisiBaglantilari.AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi Bağlantısı",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.KisiBaglantilari
            .AsNoTracking()
            .Include(kb => kb.IlgiliKisi)
            .Where(kb => kb.Id == id)
            .Select(kb => new IlgiliKisiSilVM
            {
                Id = id,
                KisiId = kb.KisiId,

                IlgiliKisiIsim = kb.IlgiliKisi.TuzelMi ?
                    kb.IlgiliKisi.SirketIsmi! :
                    $"{kb.IlgiliKisi.Isim} {kb.IlgiliKisi.Soyisim}",

                Pozisyon = kb.Pozisyon
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc> IlgiliKisiSilAsync(int id)
    {
        if (id < 1 || !await _vt.KisiBaglantilari.AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi Bağlantısı",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var model = await _vt.KisiBaglantilari.FirstAsync(kb => kb.Id == id);
        _vt.KisiBaglantilari.Remove(model);
        await _vt.SaveChangesAsync();

        return new();
    }
    #endregion

    #region KisiBelgesi
    public async Task<Sonuc<KisiBelgeleriVM>> BelgelerVMGetirAsync(
        int id, string arama, int sayfa, int sayfaBoyutu)
    {
        if (id < 1 || !await _vt.Kisiler.AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = new KisiBelgeleriVM { Id = id };

        var q = _vt.KisiBelgeleri
            .AsNoTracking()
            .Where(kb => kb.KisiId == id)
            .Select(kb => new KisiBelgeleriVM.Oge
            {
                Id = kb.Id,
                Baslik = kb.Baslik,
                Aciklama = kb.Aciklama,
                Url = kb.Url,
                OzelMi = kb.OzelMi,
                OlusturmaTarihi = kb.OlusturmaTarihi,
                Boyut = Yardimci.OkunabilirDosyaBoyutu(kb.Boyut),
                Uzanti = kb.Uzanti
            });

        if (!string.IsNullOrWhiteSpace(arama))
            q = q.Where(kb =>
                kb.Baslik.Contains(arama) ||
                (kb.Aciklama != null && kb.Aciklama.Contains(arama)) ||
                kb.Uzanti.Contains(arama));

        if (!await q.SayfaGecerliMiAsync(sayfa, sayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Sayfa",
                HataMesaji = $"Sayfa: {sayfa}, Sayfa Boyutu: {sayfaBoyutu}"
            };

        vm.ToplamSayfa = await q.ToplamSayfaAsync(sayfaBoyutu);
        vm.Sayfa = sayfa;
        vm.SayfaBoyutu = sayfaBoyutu;
        vm.Arama = arama;
        vm.Ogeler = await q.SayfaUygula(sayfa, sayfaBoyutu).ToListAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<KisiBelgesiEkleVM>> BelgeEkleVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.Kisiler.AnyAsync(k => k.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Kişi",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = new KisiBelgesiEkleVM { KisiId = id };

        return new() { Deger = vm };
    }

    public async Task<Sonuc> BelgeEkleAsync(KisiBelgesiEkleVM vm, IFormFile? belge)
    {
        if (vm.KisiId < 1 ||
            !await _vt.Kisiler.AnyAsync(k => k.Id == vm.KisiId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.KisiId} bulunamadı"
            };

        if (belge == null || belge.Length == 0)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = Sabit.Belge.HataGerekli
            };

        if (belge.Length > Sabit.Belge.MaxBoyut)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = Sabit.Belge.HataMaxBoyut
            };

        var uzanti = Path.GetExtension(belge.FileName).ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(uzanti) || !Sabit.Belge.GecerliUzantilar.Contains(uzanti))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"Yüklenebilir uzantılar: '{string.Join("', '", Sabit.Belge.GecerliUzantilar)}'"
            };

        var belgeUrl = $"belge/{Guid.NewGuid()}{uzanti}";

        var model = new KisiBelgesi
        {
            KisiId = vm.KisiId,
            Baslik = vm.Baslik,
            Aciklama = vm.Aciklama,
            OzelMi = vm.OzelMi,
            Uzanti = uzanti,
            Boyut = belge.Length,
            Url = belgeUrl
        };

        var belgeYolu = Path.Combine(_env.WebRootPath, belgeUrl);

        using (var stream = new FileStream(belgeYolu, FileMode.Create))
        {
            belge.CopyTo(stream);
        }

        model.OlusturmaTarihi = DateTime.Now;

        await _vt.KisiBelgeleri.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<KisiBelgesiSilVM>> BelgeSilVMGetirAsync(int id)
    {
        if (id < 1 || !await _vt.KisiBelgeleri.AnyAsync(kb => kb.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Belge",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.KisiBelgeleri
            .AsNoTracking()
            .Where(kb => kb.Id == id)
            .Select(kb => new KisiBelgesiSilVM
            {
                Id = id,
                Baslik = kb.Baslik,
                Aciklama = kb.Aciklama,
                Url = kb.Url,
                KisiId = kb.KisiId,
                Uzanti = kb.Uzanti,
                OlusturmaTarihi = kb.OlusturmaTarihi,
                OzelMi = kb.OzelMi,
                Boyut = Yardimci.OkunabilirDosyaBoyutu(kb.Boyut)
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> BelgeSilAsync(int id)
    {
        if (id < 1 || !await _vt.KisiBelgeleri.AnyAsync(kb => kb.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Belge",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var model = await _vt.KisiBelgeleri.FirstAsync(kb => kb.Id == id);
        var kisiId = model.KisiId;
        var belgeYolu = Path.Combine(_env.WebRootPath, model.Url);
        _vt.KisiBelgeleri.Remove(model);
        await _vt.SaveChangesAsync();

        if (File.Exists(belgeYolu))
            File.Delete(belgeYolu);

        return new() { Deger = kisiId };
    }

    public async Task<Sonuc<KisiBelgesiDuzenleVM>> BelgeDuzenleVMGetirAsync(
        int id)
    {
        if (id < 1 || !await _vt.KisiBelgeleri.AnyAsync(kb => kb.Id == id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Belge",
                HataMesaji = $"id: {id} bulunamadı"
            };

        var vm = await _vt.KisiBelgeleri
            .AsNoTracking()
            .Where(kb => kb.Id == id)
            .Select(kb => new KisiBelgesiDuzenleVM
            {
                Id = id,
                KisiId = kb.KisiId,
                Baslik = kb.Baslik,
                Aciklama = kb.Aciklama,
                OzelMi = kb.OzelMi
            })
            .FirstAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc<int>> BelgeDuzenleAsync(
        KisiBelgesiDuzenleVM vm,
        IFormFile? belge)
    {
        if (vm.Id < 1 || !await _vt.KisiBelgeleri.AnyAsync(kb => kb.Id == vm.Id))
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı"
            };

        var model = await _vt.KisiBelgeleri.FirstAsync(kb => kb.Id == vm.Id);

        model.Baslik = vm.Baslik;
        model.Aciklama = vm.Aciklama;
        model.OzelMi = vm.OzelMi;

        if (vm.BelgeyiDegistir)
        {
            if (belge == null || belge.Length == 0)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = Sabit.Belge.HataGerekli
                };

            if (belge.Length > Sabit.Belge.MaxBoyut)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = Sabit.Belge.HataMaxBoyut
                };

            var uzanti = Path.GetExtension(belge.FileName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(uzanti) || !Sabit.Belge.GecerliUzantilar.Contains(uzanti))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = string.Empty,
                    HataMesaji = $"Yüklenebilir uzantılar: '{string.Join("', '", Sabit.Belge.GecerliUzantilar)}'"
                };

            var belgeUrl = $"belge/{Guid.NewGuid()}.{uzanti}";
            var eskiBelgeYolu = Path.Combine(_env.WebRootPath, model.Url);
            var belgeYolu = Path.Combine(_env.WebRootPath, belgeUrl);

            using (var stream = new FileStream(belgeYolu, FileMode.Create))
            {
                belge.CopyTo(stream);
            }

            if (File.Exists(eskiBelgeYolu))
                File.Delete(eskiBelgeYolu);

            model.Url = belgeUrl;
            model.Uzanti = uzanti;
            model.Boyut = belge.Length;
        }

        _vt.KisiBelgeleri.Update(model);
        await _vt.SaveChangesAsync();

        return new() { Deger = model.KisiId };
    }
    #endregion
}
