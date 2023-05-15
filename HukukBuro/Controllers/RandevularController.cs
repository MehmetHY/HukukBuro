using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.Randevular;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class RandevularController : Controller
{
    #region Fields
    private readonly RandevuYoneticisi _yonetici;

    public RandevularController(RandevuYoneticisi yonetici)
    {
        _yonetici = yonetici;
    }
    #endregion

    #region Randevu
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

        vm.Kisiler = await _yonetici.KisilerGetirAsync();
        vm.Personel = await _yonetici.PersonelGetirAsync();

        return View(vm);
    }
    #endregion
}
