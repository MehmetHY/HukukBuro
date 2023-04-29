using HukukBuro.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HukukBuro.Eklentiler;

public static class KontrolcuEklentileri
{
    public static void HataEkle(this ModelStateDictionary modelState, Sonuc sonuc)
    {
        modelState.AddModelError(sonuc.HataBasligi, sonuc.HataMesaji);
    }
}
