using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.FinansIslemleri;

public class EkleVM
{
    [RegularExpression(Sabit.Regex.Para)]
    public decimal Miktar { get; set; }

    public bool Odendi { get; set; }
    public DateTime? OdemeTarhi { get; set; } = DateTime.Now;
    public DateTime? SonOdemeTarhi { get; set; } = DateTime.Now;

    public bool MakbuzKesildiMi { get; set; }
    public DateTime? MakbuzTarihi { get; set; } = DateTime.Now;
    public string? MakbuzNo { get; set; }

    public int IslemTuru { get; set; }

    public string? IslemYapanId { get; set; }
    public List<SelectListItem> Personel { get; set; } = new();

    public bool KisiBaglantisiVar { get; set; }
    public int? KisiId { get; set; }
    public List<SelectListItem> Kisiler { get; set; } = new();

    public bool DosyaBaglantisiVar { get; set; }
    public int? DosyaId { get; set; }
    public List<SelectListItem> Dosyalar { get; set; } = new();

    public bool PersonelBaglantisiVar { get; set; }
    public string? PersonelId { get; set; }

    public string? Aciklama { get; set; }

}
