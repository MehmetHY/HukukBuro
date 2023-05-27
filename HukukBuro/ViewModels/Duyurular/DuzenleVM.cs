using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Duyurular;

#pragma warning disable CS8618

public class DuzenleVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Konu { get; set; }

    [Required(ErrorMessage = Sabit.Hata.Gerekli, AllowEmptyStrings = false)]
    public string Mesaj { get; set; }

    public string? Url { get; set; }

    [Display(Name = "Bildirim gönder")]
    public bool BildirimGonder { get; set; }
}
