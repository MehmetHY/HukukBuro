using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Dosyalar;

public class TarafEkleVM
{
    public int DosyaId { get; set; }

    public int KisiId { get; set; }
    public List<SelectListItem> Kisiler { get; set; } = new();

    public bool KarsiTarafMi { get; set; }

    public int TarafTuruId { get; set; }
    public List<SelectListItem> TarafTurleri { get; set; } = new();
}
