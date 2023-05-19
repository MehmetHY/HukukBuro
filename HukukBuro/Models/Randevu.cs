using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Randevu
{
    [Key]
    public int Id { get; set; }

    public int KisiId { get; set; }
    public Kisi Kisi { get; set; }

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; }

    public bool TamamlandiMi { get; set; }

    public string? SorumluId { get; set; }
    public Personel? Sorumlu { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
}