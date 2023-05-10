using HukukBuro.Eklentiler;
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

    #region Taraf
    [HttpGet]
    [Route("[controller]/{dosyaId}/[action]")]
    public async Task<IActionResult> TarafEkle(int dosyaId)
    {
        var sonuc = await _dy.TarafEkleVMGetirAsync(dosyaId);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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

    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> TarafDuzenle(int id)
    {
        var sonuc = await _dy.TarafDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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

    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> TarafSil(int id)
    {
        var sonuc = await _dy.TarafSilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> PersonelDuzenle(int id)
    {
        var sonuc = await _dy.PersonelDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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
    [HttpGet]
    [Route("[controller]/{dosyaId}/[action]")]
    public async Task<IActionResult> DosyaBaglantisiEkle(int dosyaId)
    {
        var sonuc = await _dy.DosyaBaglantisiEkleVMGetirAsync(dosyaId);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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

    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DosyaBaglantisiDuzenle(int id)
    {
        var sonuc = await _dy.DosyaBaglantisiDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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

    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> DosyaBaglantisiSil(int id)
    {
        var sonuc = await _dy.DosyaBaglantisiSilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

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
}
