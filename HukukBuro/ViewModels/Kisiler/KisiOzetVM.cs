namespace HukukBuro.ViewModels.Kisiler;

#pragma warning disable CS8618
public class KisiOzetVM
{
    public int Id { get; set; }

    public bool TuzelMi { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? SirketIsmi { get; set; }

    public string? VergiDairesi { get; set; }

    public string? VergiNo { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Adres { get; set; }

    public string? BankaHesapBilgisi { get; set; }

    public string? EkBilgi { get; set; }

    public class KisiBaglantisi
    {
        public int Id { get; set; }

        public int KisiId { get; set; }
        public string KisiIsmi { get; set; }

        public bool SirketMi { get; set; }

        public string? Pozisyon { get; set; }
    }

    public class DosyaBaglantisi
    {
        public int DosyaId { get; set; }
        public string DosyaIsmi { get; set; }

        public string TarafTuru { get; set; }

        public bool KarsiTaraf { get; set; }
    }

    public class Belge
    {
        public int Id { get; set; }

        public string Uzanti { get; set; }

        public string Boyut { get; set; }

        public string Baslik { get; set; }

        public string Url { get; set; }

        public DateTime Tarih { get; set; }

        public string? Aciklama { get; set; }

        public bool Ozel { get; set; }
    }

    public List<KisiBaglantisi> KisiBaglantilari { get; set; } = new();
    public List<DosyaBaglantisi> DosyaBaglantilari { get; set; } = new();
    public List<Belge> Belgeler { get; set; } = new();
}
