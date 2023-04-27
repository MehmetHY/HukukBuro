using Microsoft.AspNetCore.Identity;

namespace HukukBuro.Models;

public sealed class Personel : IdentityUser
{
    public List<PersonelFinansIslemi> IlgiliFinansIslemleri { get; set; } = new();
    public List<Duyuru> OkunmamisDuyurular { get; set; } = new();

    public List<Dosya> SorumluDosyalar { get; set; } = new();
    public List<DosyaGorevi> SorumluDosyaGorevleri { get; set; } = new();
    public List<KisiGorevi> SorumluKisiGorevleri { get; set; } = new();
    public List<Randevu> SorumluRandevular { get; set; } = new();
    public List<DosyaFinansIslemi> SorumluDosyaFinansIslemi { get; set; } = new();
    public List<KisiFinansIslemi> SorumluKisiFinansIslemi { get; set; } = new();
    public List<PersonelFinansIslemi> SorumluPersonelFinansIslemi { get; set; } = new();
}
