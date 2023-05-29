using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Randevular;

#pragma warning disable CS8618

public class EkleVM
{
    [Display(Name = "Kişi")]
    public int KisiId { get; set; }
    public List<SelectListItem> Kisiler { get; set; } = new();

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Konu { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; } = DateTime.Now;

    public bool TamamlandiMi { get; set; }

    [Display(Name = "Sorumlu")]
    public string? SorumluId { get; set; }
    public List<SelectListItem> Personel { get; set; } = new();
}
