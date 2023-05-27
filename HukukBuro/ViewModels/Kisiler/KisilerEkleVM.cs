using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Kisiler;

public class KisilerEkleVM
{
    [Required]
    public bool TuzelMi { get; set; }

    [Display(Name = "İsim")]
    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    [Display(Name = "T.C. Kimlik No")]
    public string? TcKimlikNo { get; set; }

    [Display(Name = "Şirket İsmi")]
    public string? SirketIsmi { get; set; }

    [Display(Name = "Vergi No")]
    public string? VergiNo { get; set; }

    [Display(Name = "Vergi Dairesi")]
    public string? VergiDairesi { get; set; }

    [Phone]
    public string? Telefon { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Adres { get; set; }

    [Display(Name = "Banka Hesap Bilgisi")]
    public string? BankaHesapBilgisi { get; set; }

    [Display(Name = "Ek Bilgi")]
    public string? EkBilgi { get; set; }
}
