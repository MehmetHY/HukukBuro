using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Kisiler;

public class IlgiliKisiEkleVM
{
    public int KisiId { get; set; }

    public int IlgiliKisiId { get; set; }

    public List<SelectListItem> Kisiler { get; set; } = new();

    public string? Pozisyon { get; set; }
}
