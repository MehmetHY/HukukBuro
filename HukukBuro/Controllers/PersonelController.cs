using HukukBuro.Eklentiler;
using HukukBuro.Models;
using HukukBuro.ViewModels.Personeller;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
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

    [Authorize]
    public async Task<IActionResult> Listele(ListeleVM? vm)
    {
        var sonuc = await _yonetici.ListeleVMGetirAsync(vm ?? new());

        if (!sonuc.BasariliMi)
            return View(Sabit.View.Hata, sonuc);

        return View(sonuc.Deger);
    }

    [HttpGet]
    public IActionResult Kaydol()
    {
        if (_girisYoneticisi.IsSignedIn(User))
            return LocalRedirect("/");

        var vm = _yonetici.KaydolVMGetir();

        return View(vm);
    }

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

    [HttpGet]
    public IActionResult Giris(string? returnUrl)
    {
        if (_girisYoneticisi.IsSignedIn(User))
            return LocalRedirect(returnUrl ?? "/");

        return View(_yonetici.GirisVMGetir());
    }

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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Cikis(string? returnUrl)
    {
        if (_girisYoneticisi.IsSignedIn(User))
            await _girisYoneticisi.SignOutAsync();

        return LocalRedirect(returnUrl ?? "/");
    }
    #endregion

    #region personel
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profil()
    {
        var vm = await _yonetici.ProfilVMGetirAsync(User.Identity!.Name!);

        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Duzenle()
    {
        var vm = await _yonetici.DuzenleVMGetirAsync(User.Identity!.Name!);

        return View(vm);
    }

    [Authorize]
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

    [Authorize]
    [HttpGet]
    public IActionResult FotoDuzenle() => View();

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> FotoDuzenle(IFormFile? foto)
    {
        var sonuc = await _yonetici.FotoDuzenleAsync(User.Identity!.Name!, foto);

        if (sonuc.BasariliMi)
            return RedirectToAction(nameof(Profil));

        ModelState.HataEkle(sonuc);

        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> FotoKaldir()
    {
        var vm = await _yonetici.FotoVMGetirAsync(User.Identity!.Name!);

        return View(vm);
    }

    [Authorize]
    [HttpPost]
    [ActionName(nameof(FotoKaldir))]
    public async Task<IActionResult> FotoKaldirPOST()
    {
        await _yonetici.FotoSilAsync(User.Identity!.Name!);

        return RedirectToAction(nameof(Profil));
    }
    #endregion
}
