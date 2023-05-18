using HukukBuro.Data;
using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Duyurular;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Yoneticiler;

public class DuyuruYoneticisi
{
    private readonly VeriTabani _vt;

    public DuyuruYoneticisi(VeriTabani vt)
    {
        _vt = vt;
    }

    public async Task<Sonuc<ListeleVM>> ListeleVMGetirAsync(ListeleVM vm)
    {
        var q = _vt.Duyurular
            .Where(d =>
                string.IsNullOrWhiteSpace(vm.Arama) ||
                $"{d.Konu} {d.Mesaj} {d.Tarih}".Contains(vm.Arama))
            .OrderByDescending(d => d.Tarih)
            .Select(d => new OzetVM
            {
                Id = d.Id,
                Konu = d.Konu,
                Mesaj = d.Mesaj,
                Url = d.Url,
                Tarih = d.Tarih
            });

        if (!await q.SayfaGecerliMiAsync(vm.Sayfa, vm.SayfaBoyutu))
            return new()
            {
                BasariliMi = false,
                HataMesaji = "Geçersiz Sayfa",
                HataBasligi = $"Sayfa: {vm.Sayfa}, sayfa boyutu: {vm.SayfaBoyutu}"
            };

        vm.Ogeler = await q.SayfaUygula(vm.Sayfa, vm.SayfaBoyutu).ToListAsync();
        vm.ToplamSayfa = await q.ToplamSayfaAsync(vm.SayfaBoyutu);

        return new() { Deger = vm };
    }

    public EkleVM EkleVMGetir() => new();

    public async Task EkleAsync(EkleVM vm)
    {
        var model = new Duyuru
        {
            Konu = vm.Konu,
            Mesaj = vm.Mesaj,
            Tarih = DateTime.Now,
            Url = vm.Url
        };

        await _vt.Duyurular.AddAsync(model);

        var bildirimler = await _vt.Users
            .Select(u => new Bildirim
            {
                Mesaj = Sabit.Mesaj.YeniDuyuru,
                PersonelId = u.Id,
                Tarih = model.Tarih,
                Url = "/duyurular"
            })
            .ToListAsync();

        await _vt.Bildirimler.AddRangeAsync(bildirimler);
        await _vt.SaveChangesAsync();
    }
}
