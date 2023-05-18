using HukukBuro.Eklentiler;
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

    [HttpGet]
    public async Task<IActionResult> Duzenle(int id)
    {
        var sonuc = await _yonetici.DuzenleVMGetirAsync(id);

        return sonuc.BasariliMi ? View(sonuc.Deger) : View(Sabit.View.Hata, sonuc);
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

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _yonetici.OzetVMGetirAsync(id);

        return sonuc.BasariliMi ? View(sonuc.Deger) : View(Sabit.View.Hata, sonuc);
    }

    [HttpPost]
    [ActionName(nameof(Sil))]
    public async Task<IActionResult> SilPOST(int id)
    {
        var sonuc = await _yonetici.SilAsync(id);

        return sonuc.BasariliMi ?
            RedirectToAction(nameof(Listele)) : View(Sabit.View.Hata, sonuc);
    }
}
