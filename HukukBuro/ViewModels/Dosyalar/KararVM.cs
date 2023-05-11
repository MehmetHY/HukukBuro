namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class KararVM
{
    public int DosyaId { get; set; }

    public class KararBilgileriVM
    {
        public string? KararNo { get; set; }

        public DateTime? KararTarihi { get; set; }

        public DateTime? TebligTarihi { get; set; }

        public string? KararOzeti { get; set; }
    }

    public class BolgeAdliyeMahkemesiBilgileriVM
    {
        public string? KararNo { get; set; }

        public DateTime? KararTarihi { get; set; }

        public DateTime? TebligTarihi { get; set; }

        public string? KararOzeti { get; set; }

        public string? Mahkeme { get; set; }

        public DateTime? GondermeTarihi { get; set; }

        public string? Aciklama { get; set; }

        public string? EsasNo { get; set; }
    }

    public class TemyizBilgileriVM : BolgeAdliyeMahkemesiBilgileriVM { }

    public class KararDuzeltmeBilgileriVM : BolgeAdliyeMahkemesiBilgileriVM { }

    public class KesinlesmeBilgileriVM
    {
        public DateTime? KesinlesmeTarihi { get; set; }

        public string? KararOzeti { get; set; }
    }

    public KararBilgileriVM KararBilgileri { get; set; }
    public BolgeAdliyeMahkemesiBilgileriVM BolgeAdliyeMahkemesiBilgileri { get; set; }
    public TemyizBilgileriVM TemyizBilgileri { get; set; }
    public KararDuzeltmeBilgileriVM KararDuzeltmeBilgileri { get; set; }
    public KesinlesmeBilgileriVM KesinlesmeBilgileri { get; set; }
}
