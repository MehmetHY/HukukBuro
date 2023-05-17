using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public class FinansIslemi
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Miktar { get; set; }

    public DateTime? SonOdemeTarhi { get; set; }

    public bool Odendi { get; set; }
    public DateTime? OdemeTarhi { get; set; }

    public int IslemTuru { get; set; }

    public string? IslemYapanId { get; set; }
    public Personel? IslemYapan { get; set; }

    public string? Aciklama { get; set; }

    public bool MakbuzKesildiMi { get; set; }

    public DateTime? MakbuzTarihi { get; set; }

    public string? MakbuzNo { get; set; }

    public bool KisiBaglantisiVar { get; set; }
    public int? KisiId { get; set; }
    public Kisi? Kisi { get; set; }

    public bool DosyaBaglantisiVar { get; set; }
    public int? DosyaId { get; set; }
    public Dosya? Dosya { get; set; }

    public bool PersonelBaglantisiVar { get; set; }
    public string? PersonelId { get; set; }
    public Personel? Personel { get; set; }
}
