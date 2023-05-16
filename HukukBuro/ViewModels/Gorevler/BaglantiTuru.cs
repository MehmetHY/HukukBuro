using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Gorevler;

public enum BaglantiTuru
{
    [Display(Name = "Genel görev")]
    Genel,

    [Display(Name = "Dosya görevi")]
    Dosya,

    [Display(Name = "Kişi görevi")]
    Kisi
}
