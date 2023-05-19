using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class DuzenleVM
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string Isim { get; set; }

    public string Soyisim { get; set; }

    [DataType(DataType.Password)]
    public string? EskiSifre { get; set; }

    [DataType(DataType.Password)]
    public string? YeniSifre { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(YeniSifre))]
    public string? YeniSifreTekrar { get; set; }

    public bool SifreDegistir { get; set; }
}
