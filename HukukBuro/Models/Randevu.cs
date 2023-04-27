using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Randevu
{
    [Key]
    public int Id { get; set; }

    public Kisi Kisi { get; set; }

    public DateTime Tarih { get; set; }

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public Personel Sorumlu { get; set; }
}