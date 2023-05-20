using HukukBuro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HukukBuro.Controllers;
public class GenelController : Controller
{
    [Authorize]
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
