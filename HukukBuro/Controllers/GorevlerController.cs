using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.Gorevler;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class GorevlerController : Controller
{
    #region Fields
    private readonly GorevYoneticisi _yonetici;

    public GorevlerController(GorevYoneticisi yonetici)
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

    [HttpGet]
    public async Task<IActionResult> Ekle()
    {
        var vm = await _yonetici.EkleVMGetirAsync();

        return View(vm);
    }

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

        vm.Durumlar = await _yonetici.GorevDurumlariGetirAsync();
        vm.Personel = await _yonetici.PersonelGetirAsync();
        vm.Kisiler = await _yonetici.KisilerGetirAsync();
        vm.Dosyalar = await _yonetici.DosyalariGetirAsync();

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Duzenle(int id)
    {
        var sonuc = await _yonetici.DuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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

        vm.Durumlar = await _yonetici.GorevDurumlariGetirAsync();
        vm.Personel = await _yonetici.PersonelGetirAsync();
        vm.Kisiler = await _yonetici.KisilerGetirAsync();
        vm.Dosyalar = await _yonetici.DosyalariGetirAsync();

        return View(vm);
    }
}
