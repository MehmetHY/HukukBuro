using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class DuyuruKategorisi
{
    [Key]
    public int Id { get; set; }

    public string Isim { get; set; } // Onemli | Normal | UygulamaGuncellemesi
}