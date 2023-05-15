using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Randevular;

#pragma warning disable CS8618

public class EkleVM
{
    public int KisiId { get; set; }
    public List<SelectListItem> Kisiler { get; set; } = new();

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; } = DateTime.Now;

    public bool TamamlandiMi { get; set; }

    public string? SorumluId { get; set; }
    public List<SelectListItem> Personel { get; set; } = new();
}
