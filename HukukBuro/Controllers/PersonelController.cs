using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.Personeller;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class PersonelController : Controller
{
    #region Fields
    private readonly PersonelYoneticisi _py;

    public PersonelController(PersonelYoneticisi py)
    {
        _py = py;
    }
    #endregion

    public async Task<IActionResult> Listele(ListeleVM? vm)
    {
        var sonuc = await _py.ListeleVMGetirAsync(vm ?? new());

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [HttpGet]
    public IActionResult Ekle()
    {
        var vm = _py.EkleVMGetir();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Ekle(EkleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _py.EkleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        vm.Yetkiler = _py.YetkileriGetir();
        vm.AnaRoller = _py.AnaRolleriGetir();

        return View(vm);
    }
}
