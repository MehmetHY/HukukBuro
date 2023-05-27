using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class KaydolVM
{
    [EmailAddress]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Email { get; set; }

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    [Display(Name = "İsim")]
    public string Isim { get; set; }

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Soyisim { get; set; }

    public string? Telefon { get; set; }

    [Display(Name = "Şifre")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Sifre { get; set; }

    [Display(Name = "Şifre Tekrar")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    [Compare(nameof(Sifre))]
    public string SifreTekrar { get; set; }

}
