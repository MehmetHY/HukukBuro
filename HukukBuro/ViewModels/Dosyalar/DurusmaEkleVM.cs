using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Dosyalar;

public class DurusmaEkleVM
{
    public int DosyaId { get; set; }

    public int AktiviteTuruId { get; set; }
    public List<SelectListItem> AktiviteTurleri { get; set; } = new();

    public DateTime Tarih { get; set; }

    public string? Aciklama { get; set; }
}
