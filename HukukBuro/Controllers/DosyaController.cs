﻿using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.Dosyalar;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class DosyaController : Controller
{
    #region Fields
    private readonly DosyaYoneticisi _dy;

    public DosyaController(DosyaYoneticisi dy)
    {
        _dy = dy;
    }
    #endregion

    #region Dosya
    [HttpGet]
    public async Task<IActionResult> Listele(ListeleVM? vm)
    {
        var sonuc = await _dy.ListeleVMGetirAsync(vm ?? new());

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [HttpGet]
    public async Task<IActionResult> Ekle()
    {
        var vm = await _dy.EkleVMGetirAsync();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Ekle(EkleVM vm)
    {
        if (ModelState.IsValid)
        {
            await _dy.EkleAsync(vm);

            return RedirectToAction(nameof(Listele));
        }

        vm.DosyaTurleri = await _dy.DosyaTurleriGetirAsync();
        vm.DosyaKategorileri = await _dy.DosyaKategorileriGetirAsync();
        vm.DosyaDurumlari = await _dy.DosyaDurumlariGetirAsync();

        return View(vm);
    }

    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Ozet(int id)
    {
        var sonuc = await _dy.OzetVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Duzenle(int id)
    {
        var sonuc = await _dy.DuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Duzenle(DuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.DuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        vm.DosyaTurleri = await _dy.DosyaTurleriGetirAsync();
        vm.DosyaKategorileri = await _dy.DosyaKategorileriGetirAsync();
        vm.DosyaDurumlari = await _dy.DosyaDurumlariGetirAsync();

        return View(vm);
    }
    #endregion
}
