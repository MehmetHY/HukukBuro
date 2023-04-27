using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

public sealed class TarafTuru
{
    [Key]
    public int Id { get; set; }

    public string Isim { get; set; } // Davaci | Davali | Diger | Avukat | Alacakli | Borclu | Belirtilmemis
}