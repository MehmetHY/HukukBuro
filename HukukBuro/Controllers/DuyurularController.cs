using HukukBuro.ViewModels.Duyurular;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class DuyurularController : Controller
{
    private readonly DuyuruYoneticisi _yonetici;

    public DuyurularController(DuyuruYoneticisi yonetici)
    {
        _yonetici = yonetici;
    }

    [HttpGet]
    public async Task<IActionResult> Listele(ListeleVM? vm)
    {
        var sonuc = await _yonetici.ListeleVMGetirAsync(vm ?? new());

        return sonuc.BasariliMi ? View(sonuc.Deger) : View(Sabit.View.Hata, sonuc);
    }

    [HttpGet]
    public IActionResult Ekle() => View(_yonetici.EkleVMGetir());

    [HttpPost]
    public async Task<IActionResult> Ekle(EkleVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        await _yonetici.EkleAsync(vm);

        return RedirectToAction(nameof(Listele));
    }
}
