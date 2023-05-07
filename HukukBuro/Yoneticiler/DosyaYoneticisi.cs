﻿using HukukBuro.Data;
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

        return new() { Deger =  vm };
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

        return new() { Deger = vm };
    }
    #endregion
}
