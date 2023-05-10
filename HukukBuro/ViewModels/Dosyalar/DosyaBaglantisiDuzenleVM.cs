using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Dosyalar;

public class DosyaBaglantisiDuzenleVM
{
    public int Id { get; set; }

    public int DosyaId { get; set; }

    public int IlgiliDosyaId { get; set; }
    public List<SelectListItem> Dosyalar { get; set; } = new();

    public string? Aciklama { get; set; }
}
