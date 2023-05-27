using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Kisiler;

public class IlgiliKisiEkleVM
{
    public int KisiId { get; set; }

    [Display(Name = "Kişi")]
    public int IlgiliKisiId { get; set; }

    public List<SelectListItem> Kisiler { get; set; } = new();

    public string? Pozisyon { get; set; }
}
