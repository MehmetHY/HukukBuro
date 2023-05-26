using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class DosyaBelgesiEkleVM
{
    public int Id { get; set; }

    [Display(Name = "Başlık")]
    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Baslik { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }
}
