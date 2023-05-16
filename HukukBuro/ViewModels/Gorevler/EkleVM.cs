using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Gorevler;

#pragma warning disable CS8618

public class EkleVM
{
    public BaglantiTuru BaglantiTuru { get; set; }

    public int? DosyaId { get; set; }
    public List<SelectListItem> Dosyalar { get; set; } = new();

    public int? KisiId { get; set; }
    public List<SelectListItem> Kisiler { get; set; } = new();

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public DateTime BitisTarihi { get; set; } = DateTime.Now;

    public int DurumId { get; set; }
    public List<SelectListItem> Durumlar { get; set; } = new();

    public string? SorumluId { get; set; }
    public List<SelectListItem> Personel { get; set; } = new();
}
