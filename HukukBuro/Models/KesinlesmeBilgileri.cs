using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class KesinlesmeBilgileri
{
    [Key]
    public int Id { get; set; }

    public int DosyaId { get; set; }
    [ForeignKey(nameof(DosyaId))]
    public Dosya Dosya { get; set; }

    public DateTime? KesinlesmeTarihi { get; set; }

    public string? KararOzeti { get; set; }
}