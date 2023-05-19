using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Kisi
{
    [Key]
    public int Id { get; set; }

    public bool TuzelMi { get; set; }

    public string Kisaltma { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? KimlikNo { get; set; }

    public string? SirketIsmi { get; set; }

    public string? VergiDairesi { get; set; }

    public string? VergiNo { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? AdresBilgisi { get; set; }

    public string? BankaHesapBilgisi { get; set; }

    public string? EkBilgi { get; set; }


    public List<KisiBaglantisi> IlgiliKisiler { get; set; } = new();

    public List<TarafKisi> IlgiliDosyalar { get; set; } = new();

    public List<Randevu> Randevular { get; set; } = new();

    public List<Gorev> IlgiliGorevler { get; set; } = new();

    public List<FinansIslemi> IlgiliFinansIslemleri { get; set; } = new();

    public List<KisiBelgesi> Belgeler { get; set; } = new();

    [NotMapped]
    public string TamIsim  => TuzelMi ? SirketIsmi : $"{Isim} {Soyisim}";
}
