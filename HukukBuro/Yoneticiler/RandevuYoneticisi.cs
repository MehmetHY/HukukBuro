using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Randevular;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class RandevuYoneticisi
{
    #region Fields
    private readonly VeriTabani _veriTabani;

    public RandevuYoneticisi(VeriTabani vt)
    {
        _veriTabani = vt;
    }
    #endregion

    #region Utils
    public async Task<List<SelectListItem>> KisilerGetirAsync()
        => await _veriTabani.Kisiler
            .Select(k => new SelectListItem
            {
                Value = k.Id.ToString(),
                Text = k.TuzelMi ? k.SirketIsmi! : $"{k.Isim} {k.Soyisim}"
            })
            .ToListAsync();

    public async Task<List<SelectListItem>> PersonelGetirAsync()
        => await _veriTabani.Users
        .Select(u => new SelectListItem
        {
            Value = u.Id,
            Text = $"{u.Isim} {u.Soyisim}"
        })
        .ToListAsync();

    #endregion

    #region Randevu
    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _veriTabani.Randevular
            .Include(r => r.Kisi)
            .Include(r => r.Sorumlu)
            .OrderBy(r => r.TamamlandiMi)
            .ThenByDescending(r => r.Tarih)
            .Where(r =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                (r.Kisi.Isim != null && r.Kisi.Isim.Contains(vm.Arama)) ||
                (r.Kisi.Soyisim != null && r.Kisi.Soyisim.Contains(vm.Arama)) ||
                (r.Kisi.SirketIsmi != null && r.Kisi.SirketIsmi.Contains(vm.Arama)) ||
                r.Konu.Contains(vm.Arama) ||
                (r.Aciklama != null && r.Aciklama.Contains(vm.Arama)) ||
                (r.Sorumlu != null && r.Sorumlu.Isim.Contains(vm.Arama)) ||
                (r.Sorumlu != null && r.Sorumlu.Soyisim.Contains(vm.Arama)) ||
                r.Tarih.ToString().Contains(vm.Arama))
            .Select(r => new ListeleVM.Oge
            {
                Id = r.Id,

                KisiId = r.KisiId,
                Kisi = r.Kisi.TuzelMi ?
                    r.Kisi.SirketIsmi! :
                    $"{r.Kisi.Isim} {r.Kisi.Soyisim}",

                Konu = r.Konu,
                Aciklama = r.Aciklama,
                TamamlandiMi = r.TamamlandiMi,
                Tarih = r.Tarih,

                SorumluId = r.SorumluId,

                Sorumlu = r.SorumluId == null ?
                    null : $"{r.Sorumlu!.Isim} {r.Sorumlu.Soyisim}"
            });

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz sayfa",
                HataMesaji = $"Sayfa: {vm.Sayfa}, Sayfa boyutu: {vm.SayfaBoyutu}"
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
            Personel = await PersonelGetirAsync()
        };

        return vm;
    }

    public async Task<Sonuc> EkleAsync(EkleVM vm)
    {
        if (vm.KisiId < 1 || !await _veriTabani.Kisiler.AnyAsync(k => k.Id == vm.KisiId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.KisiId),
                HataMesaji = $"id: {vm.KisiId} bulunamadı."
            };

        if (!string.IsNullOrWhiteSpace(vm.SorumluId) &&
            !await _veriTabani.Users.AnyAsync(u => u.Id == vm.SorumluId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.SorumluId),
                HataMesaji = $"id: {vm.SorumluId} bulunamadı."
            };

        var model = new Randevu
        {
            Konu = vm.Konu,
            Aciklama = vm.Aciklama,
            KisiId = vm.KisiId,
            SorumluId = vm.SorumluId,
            TamamlandiMi = vm.TamamlandiMi,
            Tarih = vm.Tarih,
            OlusturmaTarihi = DateTime.Now
        };

        await _veriTabani.Randevular.AddAsync(model);

        if (!string.IsNullOrWhiteSpace(model.SorumluId))
        {
            var bildirim = new Bildirim
            {
                Mesaj = "Bir randevu sorumluluğu eklendi.",
                PersonelId = model.SorumluId,
                Tarih = DateTime.Now
            };

            await _veriTabani.Bildirimler.AddAsync(bildirim);
        }

        await _veriTabani.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<DuzenleVM>> DuzenleVMGetirAsync(int id)
    {
        var vm = await _veriTabani.Randevular
            .Where(r => r.Id == id)
            .Select(r => new DuzenleVM
            {
                Id = r.Id,
                KisiId = r.KisiId,
                Konu = r.Konu,
                Aciklama = r.Aciklama,
                Tarih = r.Tarih,
                TamamlandiMi = r.TamamlandiMi,
                SorumluId = r.SorumluId
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
        vm.Personel = await PersonelGetirAsync();

        return new() { Deger = vm };
    }

    public async Task<Sonuc> DuzenleAsync(DuzenleVM vm)
    {
        var model = await _veriTabani.Randevular.FirstOrDefaultAsync(r => r.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı."
            };

        if (vm.KisiId != model.KisiId &&
            (vm.KisiId < 1 || !await _veriTabani.Kisiler.AnyAsync(k => k.Id == vm.KisiId)))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.KisiId),
                HataMesaji = $"id: {vm.KisiId} bulunamadı."
            };

        if (vm.SorumluId != model.SorumluId &&
            !string.IsNullOrWhiteSpace(vm.SorumluId) &&
            !await _veriTabani.Users.AnyAsync(u => u.Id == vm.SorumluId))
            return new()
            {
                BasariliMi = false,
                HataBasligi = nameof(vm.SorumluId),
                HataMesaji = $"id: {vm.SorumluId} bulunamadı."
            };

        model.Konu = vm.Konu;
        model.Aciklama = vm.Aciklama;
        model.Tarih = vm.Tarih;
        model.TamamlandiMi = vm.TamamlandiMi;
        model.KisiId = vm.KisiId;

        if (model.SorumluId != vm.SorumluId)
        {
            if (!string.IsNullOrWhiteSpace(vm.SorumluId))
            {
                var bildirim = new Bildirim
                {
                    Mesaj = "Randevu sorumluluğu eklendi.",
                    PersonelId = vm.SorumluId,
                    Tarih = DateTime.Now
                };

                await _veriTabani.Bildirimler.AddAsync(bildirim);
            }

            if (!string.IsNullOrWhiteSpace(model.SorumluId))
            {
                var bildirim = new Bildirim
                {
                    Mesaj = "Randevu sorumluluğunuz kaldırıldı.",
                    PersonelId = model.SorumluId,
                    Tarih = DateTime.Now
                };

                await _veriTabani.Bildirimler.AddAsync(bildirim);
            }
        }

        model.SorumluId = vm.SorumluId;

        _veriTabani.Randevular.Update(model);
        await _veriTabani.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<SilVM>> SilVMGetirAsync(int id)
    {
        var vm = await _veriTabani.Randevular
            .Where(r => r.Id == id)
            .Include(r => r.Kisi)
            .Include(r => r.Sorumlu)
            .Select(r => new SilVM
            {
                Id = r.Id,

                Kisi = r.Kisi.TuzelMi ?
                    r.Kisi.SirketIsmi! : $"{r.Kisi.Isim} {r.Kisi.Soyisim}",

                Konu = r.Konu,
                Aciklama = r.Aciklama,
                Tarih = r.Tarih,
                TamamlandiMi = r.TamamlandiMi,

                Sorumlu = r.Sorumlu == null ?
                    null : $"{r.Sorumlu.Isim} {r.Sorumlu.Soyisim}"
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
        var model = await _veriTabani.Randevular.FirstOrDefaultAsync(r => r.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        if (!string.IsNullOrWhiteSpace(model.SorumluId))
        {
            var bildirim = new Bildirim
            {
                Mesaj = "Sorumlu olduğunuz bir randevu silindi.",
                PersonelId = model.SorumluId,
                Tarih = DateTime.Now
            };

            await _veriTabani.Bildirimler.AddAsync(bildirim);
        }

        if (!string.IsNullOrWhiteSpace(model.SorumluId))
        {
            var bildirim = new Bildirim
            {
                Mesaj = "Sorumlu olduğunuz bir randevu silindi.",
                PersonelId = model.SorumluId,
                Tarih = DateTime.Now
            };

            await _veriTabani.Bildirimler.AddAsync(bildirim);
        }

        _veriTabani.Randevular.Remove(model);
        await _veriTabani.SaveChangesAsync();

        return new();
    }
    #endregion
}
