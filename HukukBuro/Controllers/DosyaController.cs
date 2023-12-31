﻿using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.Dosyalar;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;

[Authorize]
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

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    public async Task<IActionResult> Ekle()
    {
        var vm = await _dy.EkleVMGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
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

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Duzenle(int id)
    {
        var sonuc = await _dy.DuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
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

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _dy.SilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [ActionName(nameof(Sil))]
    public async Task<IActionResult> SilPOST(int id)
    {
        var sonuc = await _dy.SilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Listele));
    }
    #endregion

    #region Taraf
    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/{dosyaId}/[action]")]
    public async Task<IActionResult> TarafEkle(int dosyaId)
    {
        var sonuc = await _dy.TarafEkleVMGetirAsync(dosyaId);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/{dosyaId}/[action]")]
    public async Task<IActionResult> TarafEkle(TarafEkleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.TarafEkleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = vm.DosyaId });

            ModelState.HataEkle(sonuc);
        }

        vm.Kisiler = await _dy.KisileriGetirAsync();
        vm.TarafTurleri = await _dy.TarafTurleriGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> TarafDuzenle(int id)
    {
        var sonuc = await _dy.TarafDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> TarafDuzenle(TarafDuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.TarafDuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }

        vm.Kisiler = await _dy.KisileriGetirAsync();
        vm.TarafTurleri = await _dy.TarafTurleriGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> TarafSil(int id)
    {
        var sonuc = await _dy.TarafSilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [ActionName(nameof(TarafSil))]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> TarafSilPOST(int id)
    {
        var sonuc = await _dy.TarafSilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });
    }
    #endregion

    #region Personel
    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> PersonelDuzenle(int id)
    {
        var sonuc = await _dy.PersonelDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> PersonelDuzenle(PersonelDuzenleVM vm)
    {
        var sonuc = await _dy.PersonelDuzenleAsync(vm);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });
    }
    #endregion

    #region DosyaBaglantisi
    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/{dosyaId}/[action]")]
    public async Task<IActionResult> DosyaBaglantisiEkle(int dosyaId)
    {
        var sonuc = await _dy.DosyaBaglantisiEkleVMGetirAsync(dosyaId);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/{dosyaId}/[action]")]
    public async Task<IActionResult> DosyaBaglantisiEkle(DosyaBaglantisiEkleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.DosyaBaglantisiEkleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }

        vm.Dosyalar = await _dy.DosyalariGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DosyaBaglantisiDuzenle(int id)
    {
        var sonuc = await _dy.DosyaBaglantisiDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DosyaBaglantisiDuzenle(DosyaBaglantisiDuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.DosyaBaglantisiDuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }

        vm.Dosyalar = await _dy.DosyalariGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DosyaBaglantisiSil(int id)
    {
        var sonuc = await _dy.DosyaBaglantisiSilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [ActionName(nameof(DosyaBaglantisiSil))]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DosyaBaglantisiSilPOST(int id)
    {
        var sonuc = await _dy.DosyaBaglantisiSilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });
    }
    #endregion

    #region Karar
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Karar(int id)
    {
        var sonuc = await _dy.KararVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> KararDuzenle(int id)
    {
        var sonuc = await _dy.KararVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> KararDuzenle(KararVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.KararDuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Karar), new { id = vm.DosyaId });

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }
    #endregion

    #region Durusma
    [HttpGet]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> Durusmalar(DurusmalarVM? vm)
    {
        var sonuc = await _dy.DurusmalarVMGetirAsync(vm ?? new());

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> DurusmaEkle(int id)
    {
        var sonuc = await _dy.DurusmaEkleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> DurusmaEkle(DurusmaEkleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.DurusmaEkleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }

        vm.AktiviteTurleri = await _dy.DurusmaAktiviteTurleriGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DurusmaDuzenle(int id)
    {
        var sonuc = await _dy.DurusmaDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DurusmaDuzenle(DurusmaDuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.DurusmaDuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }

        vm.AktiviteTurleri = await _dy.DurusmaAktiviteTurleriGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DurusmaSil(int id)
    {
        var sonuc = await _dy.DurusmaSilVMGetirAsync(id);


        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [ActionName(nameof(DurusmaSil))]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DurusmaSilPOST(int id)
    {
        var sonuc = await _dy.DurusmaSilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });
    }
    #endregion

    #region Belge
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Belgeler(DosyaBelgeleriVM? vm)
    {
        var sonuc = await _dy.DosyaBelgeleriVMGetirAsync(vm ?? new());

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> BelgeEkle(int id)
    {
        var sonuc = await _dy.BelgeEkleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> BelgeEkle(DosyaBelgesiEkleVM vm, IFormFile? belge)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.BelgeEkleAsync(vm, belge);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeDuzenle(int id)
    {
        var sonuc = await _dy.BelgeDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeDuzenle(
        DosyaBelgesiDuzenleVM vm, IFormFile? belge)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _dy.BelgeDuzenleAsync(vm, belge);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeSil(int id)
    {
        var sonuc = await _dy.BelgeSilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Dosya)]
    [HttpPost]
    [ActionName(nameof(BelgeSil))]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeSilPOST(int id)
    {
        var sonuc = await _dy.BelgeSilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Ozet), new { id = sonuc.Deger });
    }
    #endregion
}
