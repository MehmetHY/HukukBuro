using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.FinansIslemleri;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;

[Authorize]
public class FinansIslemleriController : Controller
{
    #region Fields
    private readonly FinansIslemiYoneticisi _yonetici;

    public FinansIslemleriController(FinansIslemiYoneticisi yonetici)
    {
        _yonetici = yonetici;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> Listele(ListeleVM? vm)
    {
        var sonuc = await _yonetici.ListeleVMGetirAsync(vm ?? new());

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Finans)]
    [HttpGet]
    public async Task<IActionResult> Ekle()
    {
        var vm = await _yonetici.EkleVMGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Finans)]
    [HttpPost]
    public async Task<IActionResult> Ekle(EkleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _yonetici.EkleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        vm.Personel = await _yonetici.PersonelGetirAsync();
        vm.Kisiler = await _yonetici.KisilerGetirAsync();
        vm.Dosyalar = await _yonetici.DosyalarGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Finans)]
    [HttpGet]
    public async Task<IActionResult> Duzenle(int id)
    {
        var sonuc = await _yonetici.DuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Finans)]
    [HttpPost]
    public async Task<IActionResult> Duzenle(DuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _yonetici.DuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        vm.Personel = await _yonetici.PersonelGetirAsync();
        vm.Kisiler = await _yonetici.KisilerGetirAsync();
        vm.Dosyalar = await _yonetici.DosyalarGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Finans)]
    [HttpGet]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _yonetici.SilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Finans)]
    [HttpPost]
    [ActionName(nameof(Sil))]
    public async Task<IActionResult> SilPOST(int id)
    {
        var sonuc = await _yonetici.SilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Listele));
    }
}
