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
    public IActionResult Listele()
    {
        return View();
    }
    #endregion
}
