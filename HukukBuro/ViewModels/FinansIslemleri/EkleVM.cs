using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.FinansIslemleri;

public class EkleVM
{
    [RegularExpression(Sabit.Regex.Para, ErrorMessage = Sabit.Hata.Para)]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public decimal Miktar { get; set; }

    [Display(Name = "Ödendi")]
    public bool Odendi { get; set; }
    [Display(Name = "Ödeme Tarihi")]
    public DateTime? OdemeTarhi { get; set; } = DateTime.Now;
    [Display(Name = "Son Ödeme Tarihi")]
    public DateTime? SonOdemeTarhi { get; set; } = DateTime.Now;

    [Display(Name = "Makbuz Kesildi mi?")]
    public bool MakbuzKesildiMi { get; set; }
    [Display(Name = "Makbuz Tarihi")]
    public DateTime? MakbuzTarihi { get; set; } = DateTime.Now;
    [Display(Name = "Makbuz No")]
    public string? MakbuzNo { get; set; }

    [Display(Name = "İşlem Türü")]
    public int IslemTuru { get; set; }

    [Display(Name = "İşlem Yapan")]
    public string? IslemYapanId { get; set; }
    public List<SelectListItem> Personel { get; set; } = new();

    [Display(Name = "Kişi bağlantısı var")]
    public bool KisiBaglantisiVar { get; set; }
    [Display(Name = "Kişi")]
    public int? KisiId { get; set; }
    public List<SelectListItem> Kisiler { get; set; } = new();

    [Display(Name = "Dosya bağlantısı var")]
    public bool DosyaBaglantisiVar { get; set; }
    [Display(Name = "Dosya")]
    public int? DosyaId { get; set; }
    public List<SelectListItem> Dosyalar { get; set; } = new();

    [Display(Name = "Personel bağlantısı var")]
    public bool PersonelBaglantisiVar { get; set; }
    [Display(Name = "Personel")]
    public string? PersonelId { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

}
