using HukukBuro.Eklentiler;
using HukukBuro.ViewModels;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HukukBuro.Controllers;
public class KisilerController : Controller
{
    private readonly KisiYoneticisi _ky;

    public KisilerController(KisiYoneticisi ky)
    {
        _ky = ky;
    }

    [HttpGet]
    public IActionResult Ekle() => View();

    [HttpPost]
    public async Task<IActionResult> Ekle(KisilerEkleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _ky.EkleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Listele(string arama = "", int sayfa = 1)
    {
        var sonuc = await _ky.ListeleVMGetirAsync(
            arama,
            sayfa,
            Sabitler.SayfaBoyutu);

        if (!sonuc.BasariliMi)
            return View("/hata", sonuc);

        return View(sonuc.Deger);
    }
}
