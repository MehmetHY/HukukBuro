﻿using HukukBuro.Data;
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
                d.Konu.Contains(vm.Arama) ||
                d.Mesaj.Contains(vm.Arama) ||
                d.Tarih.ToString().Contains(vm.Arama))
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

        if (vm.BildirimGonder)
        {
            var bildirimler = await _vt.Users
                .Select(u => new Bildirim
                {
                    Mesaj = Sabit.Mesaj.YeniDuyuru,
                    PersonelId = u.Id,
                    Tarih = model.Tarih,
                    Url = "/duyurular/listele"
                })
                .ToListAsync();

            await _vt.Bildirimler.AddRangeAsync(bildirimler);
        }

        await _vt.SaveChangesAsync();
    }

    public async Task<Sonuc<DuzenleVM>> DuzenleVMGetirAsync(int id)
    {
        var vm = await _vt.Duyurular
            .Where(d => d.Id == id)
            .Select(d => new DuzenleVM
            {
                Id = d.Id,
                Konu = d.Konu,
                Mesaj = d.Mesaj,
                Url = d.Url
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

    public async Task<Sonuc> DuzenleAsync(DuzenleVM vm)
    {
        var model = await _vt.Duyurular.FirstOrDefaultAsync(d => d.Id == vm.Id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = string.Empty,
                HataMesaji = $"id: {vm.Id} bulunamadı."
            };

        model.Konu = vm.Konu;
        model.Mesaj = vm.Mesaj;
        model.Url = vm.Url;

        if (vm.BildirimGonder)
        {
            model.Tarih = DateTime.Now;

            var bildirimler = await _vt.Users
                .Select(u => new Bildirim
                {
                    Mesaj = Sabit.Mesaj.YeniDuyuru,
                    PersonelId = u.Id,
                    Tarih = DateTime.Now,
                    Url = "/duyurular/listele"
                })
                .ToListAsync();

            await _vt.Bildirimler.AddRangeAsync(bildirimler);
        }

        _vt.Duyurular.Update(model);
        await _vt.SaveChangesAsync();

        return new();
    }

    public async Task<Sonuc<OzetVM>> OzetVMGetirAsync(int id)
    {
        var vm = await _vt.Duyurular
            .Where(d => d.Id == id)
            .Select(d => new OzetVM
            {
                Id = d.Id,
                Konu = d.Konu,
                Mesaj = d.Mesaj,
                Url = d.Url,
                Tarih = d.Tarih
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
        var model = await _vt.Duyurular.FirstOrDefaultAsync(d => d.Id == id);

        if (model == null)
            return new()
            {
                BasariliMi = false,
                HataBasligi = "Geçersiz Id",
                HataMesaji = $"id: {id} bulunamadı."
            };

        _vt.Duyurular.Remove(model);
        await _vt.SaveChangesAsync();

        return new();
    }
}
