using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Gorevler;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class GorevYoneticisi
{
    #region Fields 

    private readonly VeriTabani _vt;
    public GorevYoneticisi(VeriTabani vt)
    {
        _vt = vt;
    }
    #endregion

    #region Utils
    public async Task<List<SelectListItem>> GorevDurumlariGetirAsync() =>
        await _vt.GorevDurumlari.Select(g => new SelectListItem
        {
            Value = g.Id.ToString(),
            Text = g.Isim
        })
        .ToListAsync();

    public async Task<List<SelectListItem>> PersonelGetirAsync() =>
           await _vt.Users.Select(u => new SelectListItem
           {
               Value = u.Id,
               Text = $"{u.Isim} {u.Soyisim}"
           })
           .ToListAsync();

    public async Task<List<SelectListItem>> DosyalariGetirAsync() =>
           await _vt.Dosyalar.Select(d => new SelectListItem
           {
               Value = d.Id.ToString(),
               Text = $"{d.DosyaNo} {d.BuroNo} {d.Konu}"
           })
           .ToListAsync();

    public async Task<List<SelectListItem>> KisilerGetirAsync()
        => await _vt.Kisiler
        .Select(k => new SelectListItem
        {
            Value = k.Id.ToString(),
            Text = k.TuzelMi ? k.SirketIsmi! : $"{k.Isim} {k.Soyisim}"
        })
        .ToListAsync();
    #endregion

    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _vt.Gorevler
            .Where(g =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                (
                    g.Konu.Contains(vm.Arama) ||
                    g.BitisTarihi.ToString().Contains(vm.Arama) ||
                    g.OlusturmaTarihi.ToString().Contains(vm.Arama) ||
                    g.Durum.Isim.Contains(vm.Arama) ||
                    (g.Aciklama != null && g.Aciklama.Contains(vm.Arama)) ||
                    (g.KisiId != null && g.Kisi!.Isim!.Contains(vm.Arama)) ||
                    (g.KisiId != null && g.Kisi!.Soyisim!.Contains(vm.Arama)) ||
                    (g.DosyaId != null && g.Dosya!.DosyaNo.ToString().Contains(vm.Arama)) ||
                    (g.DosyaId != null && g.Dosya!.BuroNo.Contains(vm.Arama)) ||
                    (g.DosyaId != null && g.Dosya!.Konu.Contains(vm.Arama)) ||
                    (g.SorumluId != null && g.Sorumlu!.Isim.Contains(vm.Arama)) ||
                    (g.SorumluId != null && g.Sorumlu!.Soyisim.Contains(vm.Arama)) ||
                    ((BaglantiTuru)g.BaglantiTuru).ToString().Contains(vm.Arama))
                )
            .OrderBy(g => g.DurumId)
            .ThenBy(g => g.BitisTarihi)
            .ThenBy(g => g.OlusturmaTarihi)
            .Select(g => new ListeleVM.Oge
            {
                Id = g.Id,
                BaglantiTuru = (BaglantiTuru)g.BaglantiTuru,
                Konu = g.Konu,
                Aciklama = g.Aciklama,
                Durum = g.Durum.Isim,
                KisiIsmi = g.KisiId == null ? null : g.Kisi!.TamIsim,
                DosyaIsmi = g.DosyaId == null ? null : g.Dosya!.TamIsim,
                SorumluIsmi = g.SorumluId == null ? null : g.Sorumlu!.TamIsim,
                BitisTarihi = g.BitisTarihi,
                OlusturmaTarihi = g.OlusturmaTarihi
            });

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Sayfa",
                HataMesaji = $"sayfa: {vm.Sayfa}, sayfa boyutu: {vm.SayfaBoyutu}"
            };

        vm.Ogeler = await q.SayfaUygula(vm.Sayfa, vm.SayfaBoyutu).ToListAsync();
        vm.ToplamSayfa = await q.ToplamSayfaAsync(vm.SayfaBoyutu);

        return new() { Deger = vm };
    }

    public async Task<EkleVM> EkleVMGetirAsync()
    {
        var vm = new EkleVM
        {
            Kisiler = await KisilerGetirAsync(),
            Dosyalar = await DosyalariGetirAsync(),
            Personel = await PersonelGetirAsync(),
            Durumlar = await GorevDurumlariGetirAsync()
        };

        return vm;
    }

    public async Task<Sonuc> EkleAsync(EkleVM vm)
    {
        if (vm.DurumId < 1 ||
            !await _vt.GorevDurumlari.AnyAsync(gd => gd.Id == vm.DurumId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.DurumId),
                HataMesaji = $"id: {vm.DurumId} bulunamadı."
            };

        if (vm.SorumluId != null &&
            !await _vt.Users.AnyAsync(u => u.Id ==  vm.SorumluId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.SorumluId),
                HataMesaji = $"id: {vm.SorumluId} bulunamadı."
            };

        return vm.BaglantiTuru switch
        {
            BaglantiTuru.Dosya => await DosyaGoreviEkleAsync(vm),
            BaglantiTuru.Kisi => await KisiGoreviEkleAsync(vm),
            _ => await GenelGoreviEkleAsync(vm)
        };
    }
        

    public async Task<Sonuc> GenelGoreviEkleAsync(EkleVM vm)
    {
        var model = new Gorev
        {
            Konu = vm.Konu,
            Aciklama = vm.Aciklama,
            BitisTarihi = vm.BitisTarihi,
            OlusturmaTarihi = DateTime.Now,
            BaglantiTuru = (int)vm.BaglantiTuru,
            DurumId = vm.DurumId,
            SorumluId = vm.SorumluId
        };

        await _vt.Gorevler.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc> DosyaGoreviEkleAsync(EkleVM vm)
    {
        if (vm.DosyaId == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.DosyaId),
                HataMesaji = $"Gerekli."
            };

        if (vm.DosyaId < 1 &&
            !await _vt.Dosyalar.AnyAsync(d => d.Id ==  vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.DosyaId),
                HataMesaji = $"id: {vm.DosyaId} bulunamadı."
            };

        var model = new Gorev
        {
            Konu = vm.Konu,
            Aciklama = vm.Aciklama,
            BitisTarihi = vm.BitisTarihi,
            OlusturmaTarihi = DateTime.Now,
            BaglantiTuru = (int)vm.BaglantiTuru,
            DurumId = vm.DurumId,
            SorumluId = vm.SorumluId,
            DosyaId = vm.DosyaId
        };

        await _vt.Gorevler.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc> KisiGoreviEkleAsync(EkleVM vm)
    {
        if (vm.KisiId == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.KisiId),
                HataMesaji = $"Gerekli."
            };

        if (vm.KisiId < 1 &&
            !await _vt.Kisiler.AnyAsync(k => k.Id ==  vm.KisiId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.KisiId),
                HataMesaji = $"id: {vm.KisiId} bulunamadı."
            };

        var model = new Gorev
        {
            Konu = vm.Konu,
            Aciklama = vm.Aciklama,
            BitisTarihi = vm.BitisTarihi,
            OlusturmaTarihi = DateTime.Now,
            BaglantiTuru = (int)vm.BaglantiTuru,
            DurumId = vm.DurumId,
            SorumluId = vm.SorumluId,
            KisiId = vm.KisiId
        };

        await _vt.Gorevler.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<DuzenleVM>> DuzenleVMGetirAsync(int id)
    {
        var vm = await _vt.Gorevler
            .Where(g => g.Id == id)
            .Select(g => new DuzenleVM
            {
                Id = g.Id,
                Konu = g.Konu,
                Aciklama = g.Aciklama,
                BitisTarihi = g.BitisTarihi,
                DurumId = g.DurumId,
                BaglantiTuru = (BaglantiTuru)g.BaglantiTuru,
                DosyaId = g.DurumId,
                KisiId = g.KisiId,
                SorumluId = g.SorumluId
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        vm.Kisiler = await KisilerGetirAsync();
        vm.Dosyalar = await DosyalariGetirAsync();
        vm.Durumlar = await GorevDurumlariGetirAsync();
        vm.Personel= await PersonelGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc> DuzenleAsync(DuzenleVM vm)
    {
        var model = await _vt.Gorevler.FirstOrDefaultAsync(g => g.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı."
            };

        if (vm.DurumId < 1 ||
            !await _vt.GorevDurumlari.AnyAsync(gd => gd.Id == vm.DurumId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.DurumId),
                HataMesaji = $"id: {vm.DurumId} bulunamadı."
            };

        if (vm.SorumluId != null &&
            !await _vt.Users.AnyAsync(u => u.Id == vm.SorumluId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.SorumluId),
                HataMesaji = $"id: {vm.SorumluId} bulunamadı."
            };

        return vm.BaglantiTuru switch
        {
            BaglantiTuru.Dosya => await DosyaGoreviDuzenleAsync(vm, model),
            BaglantiTuru.Kisi => await KisiGoreviDuzenleAsync(vm, model),
            _ => await GenelGorevDuzenleAsync(vm, model)
        };
    }

    public async Task<Sonuc> GenelGorevDuzenleAsync(DuzenleVM vm, Gorev model)
    {
        model.BaglantiTuru = (int)BaglantiTuru.Genel;
        model.DosyaId = null;
        model.KisiId = null;
        model.Konu = vm.Konu;
        model.Aciklama = vm.Aciklama;
        model.BitisTarihi = vm.BitisTarihi;
        model.DurumId = vm.DurumId;
        model.SorumluId = vm.SorumluId;

        _vt.Gorevler.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc> DosyaGoreviDuzenleAsync(DuzenleVM vm, Gorev model)
    {
        if (vm.DosyaId < 1 ||
            !await _vt.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.DosyaId),
                HataMesaji = $"id: {vm.DosyaId} bulunamadı."
            };

        model.BaglantiTuru = (int)BaglantiTuru.Dosya;
        model.DosyaId = vm.DosyaId;
        model.KisiId = null;
        model.Konu = vm.Konu;
        model.Aciklama = vm.Aciklama;
        model.BitisTarihi = vm.BitisTarihi;
        model.DurumId = vm.DurumId;
        model.SorumluId = vm.SorumluId;

        _vt.Gorevler.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc> KisiGoreviDuzenleAsync(DuzenleVM vm, Gorev model)
    {
        if (vm.KisiId < 1 ||
            !await _vt.Kisiler.AnyAsync(k => k.Id == vm.KisiId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.KisiId),
                HataMesaji = $"id: {vm.KisiId} bulunamadı."
            };

        model.BaglantiTuru = (int)BaglantiTuru.Kisi;
        model.DosyaId = null;
        model.KisiId = vm.KisiId;
        model.Konu = vm.Konu;
        model.Aciklama = vm.Aciklama;
        model.BitisTarihi = vm.BitisTarihi;
        model.DurumId = vm.DurumId;
        model.SorumluId = vm.SorumluId;

        _vt.Gorevler.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<SilVM>> SilVMGetirAsync(int id)
    {
        var vm = await _vt.Gorevler
            .Where(g => g.Id == id)
            .Include(g => g.Kisi)
            .Include(g => g.Dosya)
            .Include(g => g.Sorumlu)
            .Include(g => g.Durum)
            .Select(g => new SilVM
            {
                Id = g.Id,
                BaglantiTuru = (BaglantiTuru)g.BaglantiTuru,
                KisiIsmi = g.KisiId == null ? null : g.Kisi!.TamIsim,
                DosyaIsmi=  g.DosyaId == null ? null : g.Dosya!.TamIsim,
                SorumluIsmi = g.SorumluId == null ? null : g.Sorumlu!.TamIsim,
                Konu = g.Konu,
                Aciklama = g.Aciklama,
                BitisTarihi = g.BitisTarihi,
                OlusturmaTarihi = g.OlusturmaTarihi,
                Durum = g.Durum.Isim
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
        var model = await _vt.Gorevler.FirstOrDefaultAsync(g => g.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        _vt.Gorevler.Remove(model);
        await _vt.SaveChangesAsync();

        return new();
    }
}
