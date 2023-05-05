using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels;

public class IlgiliKisiDuzenleVM
{
    public int Id { get; set; }

    public int KisiId { get; set; }

    public int IlgiliKisiId { get; set; }

    public List<SelectListItem> Kisiler { get; set; } = new();

    public string? Pozisyon { get; set; }
}
