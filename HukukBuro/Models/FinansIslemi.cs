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

    public DateTime SonOdemeTarhi { get; set; }

    public bool Odendi { get; set; }

    public FinansIslemTuru IslemTuru { get; set; }

    public string? IslemYapanId { get; set; }
    [ForeignKey(nameof(IslemYapanId))]
    public Personel? IslemYapan { get; set; }

    public string? Aciklama { get; set; }

    public bool MakbuzKesildiMi { get; set; }

    public DateTime? MakbuzTarihi { get; set; }

    public string? MakbuzNo { get; set; }
}
