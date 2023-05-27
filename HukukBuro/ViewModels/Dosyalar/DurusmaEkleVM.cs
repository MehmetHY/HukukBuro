using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Dosyalar;

public class DurusmaEkleVM
{
    public int DosyaId { get; set; }

    [Display(Name = "Aktivite Türü")]
    public int AktiviteTuruId { get; set; }
    public List<SelectListItem> AktiviteTurleri { get; set; } = new();

    public DateTime Tarih { get; set; } = DateTime.Now;

    [Display(Name = "Tamamlandı")]
    public bool Tamamlandi { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }
}
