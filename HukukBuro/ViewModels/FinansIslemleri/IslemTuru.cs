using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.FinansIslemleri;

public enum IslemTuru
{
    [Display(Name = "Diğer")]
    Diger,

    [Display(Name = "Serbest Meslek")]
    SerbestMeslek,

    [Display(Name = "Ofis Geliri")]
    OfisGeliri,

    [Display(Name = "Ofis Gideri")]
    OfisGideri,

    [Display(Name = "Personel Maaş")]
    PersonelMaasi,

    [Display(Name = "Personel Avans")]
    PersonelAvans,

    [Display(Name = "Dosya Geliri")]
    DosyaGeliri,

    [Display(Name = "Dosya Gideri")]
    DosyaGideri,

    [Display(Name = "İade")]
    Iade,

    [Display(Name = "Transfer")]
    Transfer
}
