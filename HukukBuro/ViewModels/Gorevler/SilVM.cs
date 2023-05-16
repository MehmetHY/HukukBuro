using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Gorevler;

#pragma warning disable CS8618

public class SilVM
{
    public int Id { get; set; }

    public BaglantiTuru BaglantiTuru { get; set; }

    public string? KisiIsmi { get; set; }
    public string? DosyaIsmi { get; set; }
    public string? SorumluIsmi { get; set; }

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public DateTime BitisTarihi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public string Durum { get; set; }
}
