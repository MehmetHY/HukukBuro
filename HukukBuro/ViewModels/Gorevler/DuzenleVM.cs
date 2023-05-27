using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Gorevler;

#pragma warning disable CS8618

public class DuzenleVM
{
    public int Id { get; set; }

    [Display(Name = "Bağlantı Türü")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public BaglantiTuru BaglantiTuru { get; set; }

    [Display(Name = "Dosya")]
    public int? DosyaId { get; set; }
    public List<SelectListItem> Dosyalar { get; set; } = new();

    [Display(Name = "Kişi")]
    public int? KisiId { get; set; }
    public List<SelectListItem> Kisiler { get; set; } = new();

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Konu { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [Display(Name = "Bitiş Tarihi")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public DateTime BitisTarihi { get; set; } = DateTime.Now;

    [Display(Name = "Durum")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public int DurumId { get; set; }
    public List<SelectListItem> Durumlar { get; set; } = new();

    [Display(Name = "Sorumlu")]
    public string? SorumluId { get; set; }
    public List<SelectListItem> Personel { get; set; } = new();
}
