using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels;
using HukukBuro.ViewModels.Personeller;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;

[Authorize]
public class PersonelController : Controller
{
    #region Fields
    private readonly PersonelYoneticisi _yonetici;
    private readonly SignInManager<Personel> _girisYoneticisi;

    public PersonelController(
        PersonelYoneticisi py,
        SignInManager<Personel> girisYoneticisi)
    {
        _yonetici = py;
        _girisYoneticisi = girisYoneticisi;
    }
    #endregion

    #region giris

    [Authorize(Policy = Sabit.Policy.Personel)]
    public async Task<IActionResult> Listele(ListeleVM? vm)
    {
        var sonuc = await _yonetici.ListeleVMGetirAsync(vm ?? new());

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Kaydol()
    {
        if (_girisYoneticisi.IsSignedIn(User))
            return LocalRedirect("/");

        var vm = _yonetici.KaydolVMGetir();

        return View(vm);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Kaydol(KaydolVM vm)
    {
        if (_girisYoneticisi.IsSignedIn(User))
            return LocalRedirect("/");

        if (ModelState.IsValid)
        {
            var sonuc = await _yonetici.KaydolAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Giris(string? returnUrl)
    {
        if (_girisYoneticisi.IsSignedIn(User))
            return LocalRedirect(returnUrl ?? "/");

        return View(_yonetici.GirisVMGetir());
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Giris(GirisVM vm, string? returnUrl)
    {
        if (_girisYoneticisi.IsSignedIn(User))
            return LocalRedirect(returnUrl ?? "/");

        if (ModelState.IsValid)
        {
            var sonuc = await _yonetici.GirisAsync(vm);

            if (sonuc.BasariliMi)
                return LocalRedirect(returnUrl ?? "/");

            if (!sonuc.Onayli)
                return View(Sabit.View.Hata, sonuc);

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Cikis(string? returnUrl)
    {
        if (_girisYoneticisi.IsSignedIn(User))
            await _girisYoneticisi.SignOutAsync();

        return LocalRedirect(returnUrl ?? "/");
    }
    #endregion

    #region personel
    [HttpGet]
    public async Task<IActionResult> Profil()
    {
        var vm = await _yonetici.ProfilVMGetirAsync(User.Identity!.Name!);

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Duzenle()
    {
        var vm = await _yonetici.DuzenleVMGetirAsync(User.Identity!.Name!);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Duzenle(DuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _yonetici.DuzenleAsync(vm, User.Identity!.Name!);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Profil));

            ModelState.HataEkle(sonuc);
        }

        return View(vm);
    }

    [HttpGet]
    public IActionResult FotoDuzenle() => View();

    [HttpPost]
    public async Task<IActionResult> FotoDuzenle(IFormFile? foto)
    {
        var sonuc = await _yonetici.FotoDuzenleAsync(User.Identity!.Name!, foto);

        if (sonuc.BasariliMi)
            return RedirectToAction(nameof(Profil));

        ModelState.HataEkle(sonuc);

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> FotoKaldir()
    {
        var vm = await _yonetici.FotoVMGetirAsync(User.Identity!.Name!);

        return View(vm);
    }

    [HttpPost]
    [ActionName(nameof(FotoKaldir))]
    public async Task<IActionResult> FotoKaldirPOST()
    {
        await _yonetici.FotoSilAsync(User.Identity!.Name!);

        return RedirectToAction(nameof(Profil));
    }

    [Authorize(Policy = Sabit.Policy.Personel)]
    [HttpGet]
    public async Task<IActionResult> YetkiDuzenle(string id)
    {
        var sonuc = await _yonetici.YetkiDuzenleVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Personel)]
    [HttpPost]
    public async Task<IActionResult> YetkiDuzenle(YetkiDuzenleVM vm)
    {
        if (ModelState.IsValid)
        {
            var sonuc = await _yonetici.YetkiDuzenleAsync(vm);

            if (sonuc.BasariliMi)
                return RedirectToAction(nameof(Listele));

            ModelState.HataEkle(sonuc);
        }

        vm.Anaroller = _yonetici.AnaRolleriGetir();

        return View(vm);
    }

    [Authorize(Policy = Sabit.Policy.Personel)]
    [HttpGet]
    public async Task<IActionResult> Sil(string id)
    {
        var sonuc = await _yonetici.OzetVMGetirAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [Authorize(Policy = Sabit.Policy.Personel)]
    [HttpPost]
    [ActionName(nameof(Sil))]
    public async Task<IActionResult> SilPOST(string id)
    {
        var sonuc = await _yonetici.SilAsync(id);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return RedirectToAction(nameof(Listele));
    }

    [HttpGet]
    public IActionResult ErisimEngellendi()
    {
        var vm = new Sonuc
        {
            HataBasligi = "Erişim Engellendi",
            HataMesaji = "Bu adrese erişim izniniz yok"
        };

        return View(Sabit.View.Hata, vm);
    }
    #endregion

    #region Bildirim
    [HttpGet]
    public async Task<IActionResult> Bildirimler(BildirimListeleVM? vm)
    {
        var sonuc = await _yonetici.BildirimListeleVMGetirAsync(
            vm ?? new(),
            User.Identity!.Name!);

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [HttpPost]
    public async Task<IActionResult> BildirimleriTemizle()
    {
        await _yonetici.BildirimleriTemizleAsync(User.Identity!.Name!);

        return RedirectToAction(nameof(Bildirimler));
    }
    #endregion
}
