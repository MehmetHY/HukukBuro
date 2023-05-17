using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.FinansIslemleri;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class FinansIslemiYoneticisi
{
    #region Fields
    private readonly VeriTabani _vt;

    public FinansIslemiYoneticisi(VeriTabani vt)
    {
        _vt = vt;
    }
    #endregion

    #region Utils
    public async Task<List<SelectListItem>> PersonelGetirAsync()
        => await _vt.Users
        .Select(u => new SelectListItem
        {
            Value = u.Id,
            Text = u.TamIsim
        })
        .ToListAsync();

    public async Task<List<SelectListItem>> KisilerGetirAsync()
        => await _vt.Kisiler
        .Select(k => new SelectListItem
        {
            Value = k.Id.ToString(),
            Text = k.TamIsim
        })
        .ToListAsync();

    public async Task<List<SelectListItem>> DosyalarGetirAsync()
        => await _vt.Dosyalar
        .Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.TamIsim
        })
        .ToListAsync();
    #endregion

    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _vt.FinansIslemleri
            .Where(f =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                f.Miktar.ToString().Contains(vm.Arama) ||
                (f.SonOdemeTarhi != null && f.SonOdemeTarhi.ToString()!.Contains(vm.Arama)) ||
                (f.Aciklama != null && f.Aciklama.Contains(vm.Arama)) ||
                (f.MakbuzTarihi != null && f.MakbuzTarihi.ToString()!.Contains(vm.Arama)) ||
                (f.MakbuzNo != null && f.MakbuzNo.Contains(vm.Arama)) ||
                (f.Kisi != null && f.Kisi.TuzelMi && f.Kisi.SirketIsmi!.Contains(vm.Arama)) ||
                (f.Kisi != null && !f.Kisi.TuzelMi && f.Kisi.Isim!.Contains(vm.Arama)) ||
                (f.Kisi != null && !f.Kisi.TuzelMi && f.Kisi.Soyisim!.Contains(vm.Arama)) ||
                (f.Dosya != null && f.Dosya.Konu.Contains(vm.Arama)) ||
                (f.Dosya != null && f.Dosya.DosyaNo.ToString().Contains(vm.Arama)) ||
                (f.Dosya != null && f.Dosya.BuroNo.Contains(vm.Arama)) ||
                (f.IslemYapan != null && f.IslemYapan.Isim.Contains(vm.Arama)) ||
                (f.IslemYapan != null && f.IslemYapan.Soyisim.Contains(vm.Arama)) ||
                (f.Personel != null && f.Personel.Isim.Contains(vm.Arama)) ||
                (f.Personel != null && f.Personel.Soyisim.Contains(vm.Arama)))
            .Select(f => new ListeleVM.Oge
            {
                Id = f.Id,
                Miktar = f.Miktar.ToString(),
                SonOdemeTarihi = f.SonOdemeTarhi,
                Odendi = f.Odendi,
                OdemeTarihi = f.OdemeTarhi,
                IslemTuru = (IslemTuru)f.IslemTuru,
                IslemYapan = f.IslemYapan == null ? null : f.IslemYapan.TamIsim,
                Aciklama = f.Aciklama,
                MakbuzKesildi = f.MakbuzKesildiMi,
                MakbuzTarihi = f.MakbuzTarihi,
                MakbuzNo = f.MakbuzNo,
                Kisi = f.Kisi == null ? null : f.Kisi.TamIsim,
                Dosya = f.Dosya == null ? null : f.Dosya.TamIsim,
                Personel = f.Personel == null ? null : f.Personel.TamIsim
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
            Personel = await PersonelGetirAsync(),
            Kisiler = await KisilerGetirAsync(),
            Dosyalar = await DosyalarGetirAsync()
        };

        return vm;
    }

    public async Task<Sonuc> EkleAsync(EkleVM vm)
    {
        var model = new FinansIslemi
        {
            Miktar = vm.Miktar,

            Odendi = vm.Odendi,
            MakbuzKesildiMi = vm.Odendi && vm.MakbuzKesildiMi,

            IslemTuru = vm.IslemTuru,

            IslemYapanId = vm.IslemYapanId,

            Aciklama = vm.Aciklama,

            KisiBaglantisiVar = vm.KisiBaglantisiVar,
            DosyaBaglantisiVar = vm.DosyaBaglantisiVar,
            PersonelBaglantisiVar = vm.PersonelBaglantisiVar
        };

        if (vm.IslemYapanId != null &&
            !await _vt.Users.AnyAsync(u => u.Id == vm.IslemYapanId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.IslemYapanId),
                HataMesaji = $"id: {vm.IslemYapanId} bulunamadı."
            };

        if (vm.PersonelBaglantisiVar)
        {
            if (vm.PersonelId == null)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.PersonelId),
                    HataMesaji = "Gerekli."
                };

            if (!await _vt.Users.AnyAsync(u => u.Id == vm.PersonelId))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.PersonelId),
                    HataMesaji = $"id: {vm.PersonelId} bulunamadı."
                };

            model.PersonelId = vm.PersonelId;
        }

        if (vm.KisiBaglantisiVar)
        {
            if (vm.KisiId == null)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.KisiId),
                    HataMesaji = "Gerekli."
                };

            if (!await _vt.Kisiler.AnyAsync(k => k.Id == vm.KisiId))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.KisiId),
                    HataMesaji = $"id: {vm.KisiId} bulunamadı."
                };

            model.KisiId = vm.KisiId;
        }

        if (vm.DosyaBaglantisiVar)
        {
            if (vm.DosyaId == null)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.DosyaId),
                    HataMesaji = "Gerekli."
                };

            if (!await _vt.Dosyalar.AnyAsync(d => d.Id == vm.DosyaId))
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.DosyaId),
                    HataMesaji = $"id: {vm.DosyaId} bulunamadı."
                };

            model.DosyaId = vm.DosyaId;
        }

        if (vm.Odendi)
        {
            if (vm.OdemeTarhi == null)
                return new()
                {
                    BasariliMi = false,
                    HataBasligi = nameof(vm.OdemeTarhi),
                    HataMesaji = "Gerekli."
                };

            model.OdemeTarhi = vm.OdemeTarhi;

            if (vm.MakbuzKesildiMi)
            {
                if (vm.MakbuzTarihi == null)
                    return new()
                    {
                        BasariliMi = false,
                        HataBasligi = nameof(vm.MakbuzTarihi),
                        HataMesaji = "Gerekli."
                    };

                if (string.IsNullOrWhiteSpace(vm.MakbuzNo))
                    return new()
                    {
                        BasariliMi = false,
                        HataBasligi = nameof(vm.MakbuzNo),
                        HataMesaji = "Gerekli."
                    };

                model.MakbuzTarihi = vm.MakbuzTarihi;
                model.MakbuzNo = vm.MakbuzNo;
            }
        }
        else
        {
            model.SonOdemeTarhi = vm.SonOdemeTarhi;
        }

        await _vt.FinansIslemleri.AddAsync(model);
        await _vt.SaveChangesAsync();

        return new();
    }
}
