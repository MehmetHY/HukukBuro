using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels;

public class KisilerEkleVM
{
    [Required]
    public bool TuzelMi { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? SirketIsmi { get; set; }

    public string? VergiNo { get; set; }

    public string? VergiDairesi { get; set; }

    [Phone]
    public string? Telefon { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Adres { get; set; }

    public string? BankaHesapBilgisi { get; set; }

    public string? EkBilgi { get; set; }
}
