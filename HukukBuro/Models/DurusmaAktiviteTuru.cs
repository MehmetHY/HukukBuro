using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class DurusmaAktiviteTuru
{
    [Key]
    public int Id { get; set; }

    public string Isim { get; set; } // Durusma | Kesif | Inceleme
}