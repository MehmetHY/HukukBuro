using HukukBuro.Eklentiler;
using HukukBuro.ViewModels.Kisiler;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;

[Authorize]
public class KisilerController : Controller
{
    #region Fields
    private readonly KisiYoneticisi _ky;

    public KisilerController(KisiYoneticisi ky)
    {
        _ky = ky;
    }
    #endregion

    #region Kisi
    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    public IActionResult Ekle() => View();

    [Authorize(Policy = Sabit.Policy.Kisi)]
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
    public async Task<IActionResult> Listele(
        string arama = "",
        int sayfa = 1,
        int sayfaBoyutu = Sabit.SayfaBoyutu)
    {
        var sonuc = await _ky.ListeleVMGetirAsync(
            arama,
            sayfa,
            sayfaBoyutu);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Ozet(int id)
    {
        var sonuc = await _ky.OzetVMGetirAsync(id);

        if (sonuc.BasariliMi)
            return View(sonuc.Deger);

        return View(Sabit.View.Hata, sonuc);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _ky.SilVMGetirAsync(id);

        if (sonuc.BasariliMi)
            return View(sonuc.Deger);

        return View(Sabit.View.Hata, sonuc);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [ActionName("Sil")]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> SilPOST(int id)
    {
        var sonuc = await _ky.SilAsync(id);

        if (sonuc.BasariliMi)
            return RedirectToAction(nameof(Listele));

        return View(Sabit.View.Hata, sonuc);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> OzetDuzenle(int id)
    {
        var sonuc = await _ky.OzetDuzenleVMGetirAsync(id);

        if (sonuc.BasariliMi)
            return View(sonuc.Deger);

        return View(Sabit.View.Hata, sonuc);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> OzetDuzenle(KisiOzetDuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _ky.OzetDuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }
    #endregion

    #region KisiBaglantisi
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> IlgiliKisiler(
        int id,
        string arama = "",
        int sayfa = 1,
        int sayfaBoyutu = Sabit.SayfaBoyutu)
    {
        var sonuc = await _ky.IlgiliKisilerVMGetirAsync(
            id,
            arama,
            sayfa,
            sayfaBoyutu);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> IlgiliKisiEkle(int id)
    {
        var sonuc = await _ky.IlgiliKisiEkleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> IlgiliKisiEkle(IlgiliKisiEkleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _ky.IlgiliKisiEkleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(IlgiliKisiler), new { id = vm.KisiId });

            ModelState.HataEkle(sonuc);
        }

        vm.Kisiler = await _ky.KisilerSelectListItemGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> IlgiliKisiDuzenle(int id)
    {
        var sonuc = await _ky.IlgiliKisiDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> IlgiliKisiDuzenle(IlgiliKisiDuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _ky.IlgiliKisiDuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(IlgiliKisiler), new { id = vm.KisiId });

            ModelState.HataEkle(sonuc);
        }

        vm.Kisiler = await _ky.KisilerSelectListItemGetirAsync();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> IlgiliKisiSil(int id)
    {
        var sonuc = await _ky.IlgiliKisiSilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> IlgiliKisiSil(IlgiliKisiSilVM vm)
    {
        var sonuc = await _ky.IlgiliKisiSilAsync(vm.Id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(IlgiliKisiler), new { id = vm.KisiId });
    }
    #endregion

    #region KisiBelgesi
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> Belgeler(
        int id,
        string arama = "",
        int sayfa = 1,
        int sayfaBoyutu = Sabit.SayfaBoyutu)
    {
        var sonuc = await _ky.BelgelerVMGetirAsync(id, arama, sayfa, sayfaBoyutu);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> BelgeEkle(int id)
    {
        var sonuc = await _ky.BelgeEkleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [Route("[controller]/{id}/[action]")]
    public async Task<IActionResult> BelgeEkle(KisiBelgesiEkleVM vm, IFormFile? belge)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _ky.BelgeEkleAsync(vm, belge);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Belgeler), new { id = vm.KisiId });

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeSil(int id)
    {
        var sonuc = await _ky.BelgeSilVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [ActionName(nameof(BelgeSil))]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeSilPOST(int id)
    {
        var sonuc = await _ky.BelgeSilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Belgeler), new { id = sonuc.Deger });
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeDuzenle(int id)
    {
        var sonuc = await _ky.BelgeDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Kisi)]
    [HttpPost]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BelgeDuzenle(
        KisiBelgesiDuzenleVM vm,
        IFormFile? belge)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _ky.BelgeDuzenleAsync(vm, belge);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Belgeler), new { id = sonuc.Deger });

            ModelState.HataEkle(sonuc);
        }    

        return View(vm);
    }
    #endregion
}
