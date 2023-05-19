using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Dosya
{
    [Key]
    public int Id { get; set; }

    public string DosyaNo { get; set; }

    public string BuroNo { get; set; }

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public int DosyaTuruId { get; set; }
    public DosyaTuru DosyaTuru { get; set; }

    public int DosyaKategorisiId { get; set; }
    public DosyaKategorisi DosyaKategorisi { get; set; }

    public int DosyaDurumuId { get; set; }
    public DosyaDurumu DosyaDurumu { get; set; }

    public string? Mahkeme { get; set; }

    public DateTime AcilisTarihi { get; set; }

    public List<TarafKisi> Taraflar { get; set; } = new();

    public List<DosyaPersonel> SorumluPersonel { get; set; } = new();

    public List<DosyaBaglantisi> IlgiliDosyalar { get; set; } = new();

    public List<Gorev> IlgiliGorevler { get; set; } = new();

    public List<Durusma> Durusmalar { get; set; } = new();

    public List<DosyaBelgesi> Belgeler { get; set; } = new();

    public List<FinansIslemi> IlgiliFinansIslemleri { get; set; } = new();

    public KararBilgileri? KararBilgileri { get; set; }

    public BolgeAdliyeMahkemesiBilgileri? BolgeAdliyeMahkemesiBilgileri { get; set; }

    public TemyizBilgileri? TemyizBilgileri { get; set; }

    public KararDuzeltmeBilgileri? KararDuzeltmeBilgileri { get; set; }

    public KesinlesmeBilgileri? KesinlesmeBilgileri { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    [NotMapped]
    public string TamIsim => $"{DosyaNo} {BuroNo} {Konu}";
}
