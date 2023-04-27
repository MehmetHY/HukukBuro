using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

public class MahkemeBilgileriBase
{
    [Key]
    public int Id { get; set; }

    public int DosyaId { get; set; }
    public Dosya Dosya { get; set; }

    public string? KararNo { get; set; }

    public DateTime? KararTarihi { get; set; }

    public DateTime? TebligTarihi { get; set; }

    public string? KararOzeti { get; set; }
    public string? Mahkeme { get; set; }

    public DateTime? GondermeTarihi { get; set; }

    public string? Aciklama { get; set; }

    public string? EsasNo { get; set; }

}
