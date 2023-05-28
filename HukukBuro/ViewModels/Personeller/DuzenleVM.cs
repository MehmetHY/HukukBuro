using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class DuzenleVM
{
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Email { get; set; }

    [Display(Name = "İsim")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Isim { get; set; }

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Soyisim { get; set; }

    public string? Telefon { get; set; }

    [Display(Name = "Eski Şifre")]
    [DataType(DataType.Password)]
    public string? EskiSifre { get; set; }

    [Display(Name = "Yeni Şifre")]
    [DataType(DataType.Password)]
    public string? YeniSifre { get; set; }

    [Display(Name = "Yeni Şifre Tekrar")]
    [DataType(DataType.Password)]
    [Compare(nameof(YeniSifre))]
    public string? YeniSifreTekrar { get; set; }

    public bool SifreDegistir { get; set; }
}
