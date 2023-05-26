using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class EkleVM
{
    [Display(Name = "Dosya No")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string DosyaNo { get; set; }

    [Display(Name = "Büro No")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string BuroNo { get; set; }

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Konu { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [Display(Name = "Dosya Türü")]
    public int DosyaTuruId { get; set; }
    [ValidateNever]
    public List<SelectListItem> DosyaTurleri { get; set; } = new();

    [Display(Name = "Kategori")]
    public int DosyaKategorisiId { get; set; }
    [ValidateNever]
    public List<SelectListItem> DosyaKategorileri { get; set; } = new();

    [Display(Name = "Durum")]
    public int DosyaDurumuId { get; set; }
    [ValidateNever]
    public List<SelectListItem> DosyaDurumlari { get; set; } = new();

    public string? Mahkeme { get; set; }

    [Display(Name = "Açılış Tarihi")]
    public DateTime AcilisTarihi { get; set; } = DateTime.Now;
}
