using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class KaydolVM
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string Isim { get; set; }

    public string Soyisim { get; set; }

    [DataType(DataType.Password)]
    public string Sifre { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Sifre))]
    public string SifreTekrar { get; set; }
}
