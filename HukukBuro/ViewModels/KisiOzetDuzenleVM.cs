using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels;

public class KisiOzetDuzenleVM
{
    public int Id { get; set; }

    [Required]
    public bool TuzelMi { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? SirketIsmi { get; set; }

    public string? VergiNo { get; set; }

    public string? VergiDairesi { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Adres { get; set; }

    public string? BankaHesapBilgisi { get; set; }

    public string? EkBilgi { get; set; }
}
