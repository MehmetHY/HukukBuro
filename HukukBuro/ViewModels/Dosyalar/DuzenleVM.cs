using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class DuzenleVM
{
    public int Id { get; set; }

    public int DosyaNo { get; set; }

    public string BuroNo { get; set; }

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public int DosyaTuruId { get; set; }
    [ValidateNever]
    public List<SelectListItem> DosyaTurleri { get; set; } = new();

    public int DosyaKategorisiId { get; set; }
    [ValidateNever]
    public List<SelectListItem> DosyaKategorileri { get; set; } = new();

    public int DosyaDurumuId { get; set; }
    [ValidateNever]
    public List<SelectListItem> DosyaDurumlari { get; set; } = new();

    public string? Mahkeme { get; set; }

    public DateTime AcilisTarihi { get; set; }
}
