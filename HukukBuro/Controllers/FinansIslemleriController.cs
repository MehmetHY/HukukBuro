using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.FinansIslemleri;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
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

        vm.Personel = await _yonetici.PersonelGetirAsync();
        vm.Kisiler = await _yonetici.KisilerGetirAsync();
        vm.Dosyalar = await _yonetici.DosyalarGetirAsync();

        return View(vm);
    }
}
