using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class TarafKisi
{
    [Key]
    public int Id { get; set; }

    public int KisiId { get; set; }
    public Kisi Kisi { get; set; }

    public int DosyaId { get; set; }
    public Dosya Dosya { get; set; }

    public bool KarsiTaraf { get; set; }

    public int TarafTuruId { get; set; }
    public TarafTuru TarafTuru { get; set; }
}