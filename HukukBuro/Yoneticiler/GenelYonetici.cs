using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.Genel;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class GenelYonetici
{
    private readonly VeriTabani _veriTabani;

    public GenelYonetici(VeriTabani veriTabani)
    {
        _veriTabani = veriTabani;
    }

    public async Task<AnasayfaVM> AnasayfaVMGetirAsync()
    {
        var duyurular = await _veriTabani.Duyurular
            .OrderByDescending(d => d.Tarih)
            .Take(5)
            .Select(d => new AnasayfaVM.Duyuru
            {
                Id = d.Id,
                Konu = d.Konu,
                Mesaj = d.Mesaj,
                Tarih = d.Tarih,
                Url = d.Url
            })
            .ToListAsync();

        var gorevler = await _veriTabani.Gorevler
            .OrderByDescending(g => g.OlusturmaTarihi)
            .Take(5)
            .Include(g => g.Durum)
            .Include(g => g.Kisi)
            .Include(g => g.Dosya)
            .Include(g => g.Sorumlu)
            .Select(g => new AnasayfaVM.Gorev
            {
                Id = g.Id,
                Konu = g.Konu,
                Aciklama = g.Aciklama,
                BaglantiTuru = ((ViewModels.Gorevler.BaglantiTuru)g.BaglantiTuru).DisplayName(),
                Durum = g.Durum.Isim,
                KisiId = g.KisiId,
                Kisi = g.KisiId == null ? null : g.Kisi!.TamIsim,
                DosyaId = g.DosyaId,
                Dosya = g.DosyaId == null ? null : g.Dosya!.TamIsim,
                SorumluId = g.SorumluId,
                Sorumlu = g.SorumluId == null ? null : g.Sorumlu!.TamIsim,
                BitisTarihi = g.BitisTarihi,
                OlusturmaTarihi = g.OlusturmaTarihi
            })
            .ToListAsync();

        var randevular = await _veriTabani.Randevular
            .OrderByDescending(r => r.OlusturmaTarihi)
            .Take(5)
            .Include(r => r.Sorumlu)
            .Select(r => new AnasayfaVM.Randevu
            {
                Id = r.Id,
                KisiId = r.KisiId,
                Kisi = r.Kisi.TamIsim,
                Konu = r.Konu,
                Aciklama = r.Aciklama,
                Tarih = r.Tarih,
                SorumluId = r.SorumluId,
                Sorumlu = r.SorumluId == null ? null : r.Sorumlu!.TamIsim,
                TamamlandiMi = r.TamamlandiMi
            })
            .ToListAsync();

        var durusmalar = await _veriTabani.Durusmalar
            .OrderByDescending(d => d.Tarih)
            .Take(5)
            .Include(d => d.Dosya)
            .Include(d => d.AktiviteTuru)
            .Select(d => new AnasayfaVM.Durusma
            {
                Id = d.Id,
                DosyaId = d.DosyaId,
                Dosya = d.Dosya.TamIsim,
                AktiviteTuru = d.AktiviteTuru.Isim,
                Aciklama = d.Aciklama,
                Tarih = d.Tarih,
                Tamamlandi = d.Tamamlandi
            })
            .ToListAsync();

        var finansIslemleri = await _veriTabani.FinansIslemleri
            .OrderByDescending(f => f.OlusturmaTarihi)
            .Take(5)
            .Select(f => new AnasayfaVM.FinansIslemi
            {
                Id = f.Id,
                Miktar = $"{f.Miktar:F2} TL",
                Odendi = f.Odendi,
                SonOdemeTarhi = f.SonOdemeTarhi,
                OdemeTarhi = f.OdemeTarhi,
                MakbuzKesildiMi = f.MakbuzKesildiMi,
                MakbuzNo = f.MakbuzNo,
                MakbuzTarihi = f.MakbuzTarihi,
                IslemTuru = ((ViewModels.FinansIslemleri.IslemTuru)f.IslemTuru).DisplayName(),
                KisiId = f.KisiId,
                Kisi = f.KisiId == null ? null : f.Kisi!.TamIsim,
                DosyaId = f.DosyaId,
                Dosya = f.DosyaId == null ? null : f.Dosya!.TamIsim,
                PersonelId = f.PersonelId,
                Personel = f.PersonelId == null ? null : f.Personel!.TamIsim,
                IslemYapanId = f.IslemYapanId,
                IslemYapan = f.IslemYapan == null ? null : f.IslemYapan!.TamIsim,
                Aciklama = f.Aciklama
            })
            .ToListAsync();

        return new()
        {
            Duyurular = duyurular,
            Gorevler = gorevler,
            Randevular = randevular,
            Durusmalar = durusmalar,
            FinansIslemleri = finansIslemleri
        };
    }
}
