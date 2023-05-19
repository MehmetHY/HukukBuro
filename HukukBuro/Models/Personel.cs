using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Personel : IdentityUser
{
    public string Isim { get; set; }

    public string Soyisim { get; set; }

    public string FotoUrl { get; set; }

    public List<Bildirim> Bildirimler { get; set; } = new();
    public List<FinansIslemi> IlgiliFinansIslemleri { get; set; } = new();
    public List<DosyaPersonel> SorumluDosyalar { get; set; } = new();
    public List<Gorev> SorumluGorevler { get; set; } = new();
    public List<Randevu> SorumluRandevular { get; set; } = new();
    public List<FinansIslemi> SorumluFinansIslemleri { get; set; } = new();

    [NotMapped]
    public string TamIsim => $"{Isim} {Soyisim}";
}
