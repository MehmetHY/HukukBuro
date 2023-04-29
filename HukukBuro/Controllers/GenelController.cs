using HukukBuro.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class GenelController : Controller
{
    [HttpGet]
    public IActionResult Anasayfa()
    {
        return View();
    }

    public IActionResult Hata()
    {
        return View();
    }
}
