using HukukBuro.ViewModels;
using HukukBuro.Yoneticiler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class GenelController : Controller
{
    private readonly GenelYonetici _yonetici;

    public GenelController(GenelYonetici yonetici)
    {
        _yonetici = yonetici;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Anasayfa()
    {
        var vm = await _yonetici.AnasayfaVMGetirAsync();

        return View(vm);
    }

    public IActionResult Hata()
    {
        return View();
    }
}
