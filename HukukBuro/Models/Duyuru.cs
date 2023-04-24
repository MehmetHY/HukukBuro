using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Duyuru
{
    [Key]
    public int Id { get; set; }

    public DateTime Tarih { get; set; } = DateTime.Now;

    public string Mesaj { get; set; }

    public DuyuruKategorisi Kategori { get; set; }

    public List<Personel> Okumayanlar { get; set; } = new();
}