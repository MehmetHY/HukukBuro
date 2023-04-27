using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public class KararBilgileri
{
    [Key]
    public int Id { get; set; }

    public int DosyaId { get; set; }
    public Dosya Dosya { get; set; }

    public string? KararNo { get; set; }

    public DateTime? KararTarihi { get; set; }

    public DateTime? TebligTarihi { get; set; }

    public string? KararOzeti { get; set; }
}