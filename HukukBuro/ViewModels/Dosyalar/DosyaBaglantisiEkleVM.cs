using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Dosyalar;

public class DosyaBaglantisiEkleVM
{
    public int DosyaId { get; set; }

    [Display(Name = "Dosya")]
    public int IlgiliDosyaId { get; set; }
    public List<SelectListItem> Dosyalar { get; set; } = new();

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }
}
