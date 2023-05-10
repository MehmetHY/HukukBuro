using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class DosyaBaglantisi
{
    [Key]
    public int Id { get; set; }

    public int DosyaId { get; set; }
    public Dosya Dosya { get; set; }

    public int IlgiliDosyaId { get; set; }
    public Dosya IlgiliDosya { get; set; }

    public string? Aciklama { get; set; }
}
