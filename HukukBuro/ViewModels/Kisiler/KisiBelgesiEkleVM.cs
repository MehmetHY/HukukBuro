using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Kisiler;

#pragma warning disable CS8618

public class KisiBelgesiEkleVM
{
    public int KisiId { get; set; }

    [Display(Name = "Başlık")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Baslik { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [Display(Name = "Özel mi?")]
    public bool OzelMi { get; set; }
}
