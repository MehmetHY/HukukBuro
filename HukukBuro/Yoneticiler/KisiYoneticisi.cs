using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class KisiYoneticisi
{
    private readonly VeriTabani _vt;

    public KisiYoneticisi(VeriTabani vt)
    {
        _vt = vt;
    }

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
                $"{vm.SirketIsmi[0]}{vm.SirketIsmi[0]}".ToUpper() :
                $"{vm.SirketIsmi[..2]}".ToUpper(),

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
            sonuc.HataMesaji = $"sayfa = {sayfa}, sayfa boyutu = {sayfaBoyutu}";

            return sonuc;
        }

        sonuc.Deger = new();
        sonuc.Deger.ToplamSayfa = await q.CountAsync();
        sonuc.Deger.Sayfa = sayfa;

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
                    g => g.Durum.Isim == Sabitler.GorevDurumu.DevamEdiyor).Count(),

                FinansSayisi = k.IlgiliFinansIslemleri.Where(f => !f.Odendi).Count()
            })
            .ToListAsync();

        return sonuc;
    }
}
