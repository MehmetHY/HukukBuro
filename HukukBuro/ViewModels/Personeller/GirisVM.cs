using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class GirisVM
{
    [EmailAddress]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Email { get; set; }

    [Display(Name = "Şifre")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Sifre { get; set; }

    [Display(Name = "Beni hatırla")]
    public bool Hatirla { get; set; }
}
