using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public class Gorev
{
    [Key]
    public int Id { get; set; }

    public int BaglantiTuru { get; set; }

    public int? KisiId { get; set; }
    public Kisi? Kisi { get; set; }

    public int? DosyaId { get; set; }
    public Dosya? Dosya { get; set; }

    public string Konu { get; set; }

    public string? SorumluId { get; set; }
    public Personel? Sorumlu { get; set; }

    public DateTime BitisTarihi { get; set; }

    public string? Aciklama { get; set; }

    public int DurumId { get; set; }
    public GorevDurumu Durum { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
}
