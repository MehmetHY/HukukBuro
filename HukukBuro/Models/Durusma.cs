using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Durusma
{
    [Key]
    public int Id { get; set; }

    public int DosyaId { get; set; }
    public Dosya Dosya { get; set; }

    public int AktiviteTuruId { get; set; }
    public DurusmaAktiviteTuru AktiviteTuru { get; set; }

    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; }

    public bool Tamamlandi { get; set; }
}
