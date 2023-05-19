using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class GirisVM
{
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Sifre { get; set; }

    public bool Hatirla { get; set; }
}
