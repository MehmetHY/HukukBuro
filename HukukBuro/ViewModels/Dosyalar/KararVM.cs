using System.ComponentModel.DataAnnotations;

namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class KararVM
{
    public int DosyaId { get; set; }

    public class KararBilgileriVM
    {
        [Display(Name = "Karar No")]
        public string? KararNo { get; set; }

        [Display(Name = "Karar Tarihi")]
        public DateTime? KararTarihi { get; set; }

        [Display(Name = "Tebliğ Tarihi")]
        public DateTime? TebligTarihi { get; set; }

        [Display(Name = "Karar Özeti")]
        public string? KararOzeti { get; set; }
    }

    public class BolgeAdliyeMahkemesiBilgileriVM
    {
        [Display(Name = "Karar No")]
        public string? KararNo { get; set; }

        [Display(Name = "Karar Tarihi")]
        public DateTime? KararTarihi { get; set; }

        [Display(Name = "Tebliğ Tarihi")]
        public DateTime? TebligTarihi { get; set; }

        [Display(Name = "Karar Özeti")]
        public string? KararOzeti { get; set; }

        public string? Mahkeme { get; set; }

        [Display(Name = "Gönderme Tarihi")]
        public DateTime? GondermeTarihi { get; set; }

        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }

        [Display(Name = "Esas No")]
        public string? EsasNo { get; set; }
    }

    public class TemyizBilgileriVM : BolgeAdliyeMahkemesiBilgileriVM { }

    public class KararDuzeltmeBilgileriVM : BolgeAdliyeMahkemesiBilgileriVM { }

    public class KesinlesmeBilgileriVM
    {
        [Display(Name = "Kesinleşme Tarihi")]
        public DateTime? KesinlesmeTarihi { get; set; }

        [Display(Name = "Karar Özeti")]
        public string? KararOzeti { get; set; }
    }

    public KararBilgileriVM KararBilgileri { get; set; }
    public BolgeAdliyeMahkemesiBilgileriVM BolgeAdliyeMahkemesiBilgileri { get; set; }
    public TemyizBilgileriVM TemyizBilgileri { get; set; }
    public KararDuzeltmeBilgileriVM KararDuzeltmeBilgileri { get; set; }
    public KesinlesmeBilgileriVM KesinlesmeBilgileri { get; set; }
}
