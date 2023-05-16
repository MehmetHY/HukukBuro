﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Personel : IdentityUser
{
    public string Isim { get; set; }

    public string Soyisim { get; set; }

    public List<PersonelFinansIslemi> IlgiliFinansIslemleri { get; set; } = new();
    public List<Duyuru> OkunmamisDuyurular { get; set; } = new();

    public List<DosyaPersonel> SorumluDosyalar { get; set; } = new();
    public List<Gorev> SorumluGorevler { get; set; } = new();
    public List<Randevu> SorumluRandevular { get; set; } = new();
    public List<DosyaFinansIslemi> SorumluDosyaFinansIslemi { get; set; } = new();
    public List<KisiFinansIslemi> SorumluKisiFinansIslemi { get; set; } = new();
    public List<PersonelFinansIslemi> SorumluPersonelFinansIslemi { get; set; } = new();

    [NotMapped]
    public string TamIsim => $"{Isim} {Soyisim}";
}
