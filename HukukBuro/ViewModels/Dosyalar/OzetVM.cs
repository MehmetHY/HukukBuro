namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class OzetVM
{
    public class Taraf
    {
        public int Id { get; set; }

        public string Isim { get; set; }

        public string TarafTuru { get; set; }

        public bool KarsiTaraf { get; set; }

        public int KisiId { get; set; }
    }

    public class Personel
    {
        public string Id { get; set; }

        public string TamIsim { get; set; }

        public string AnaRol { get; set; }
    }

    public class Baglanti
    {
        public int Id { get; set; }

        public int IlgiliDosyaId { get; set; }

        public string Dosya { get; set; }
        public string Tur { get; set; }
        public string Kategori { get; set; }
        public string Durum { get; set; }

        public string? Aciklama { get; set; }
    }

    public class Durusma
    {
        public int Id { get; set; }

        public string AktiviteTuru { get; set; }

        public DateTime Tarih { get; set; }

        public string? Aciklama { get; set; }

        public bool Tamamlandi { get; set; }
    }

    public class Belge
    {
        public int Id { get; set; }

        public string Uzanti { get; set; }

        public string Boyut { get; set; }

        public string Baslik { get; set; }

        public DateTime Tarih { get; set; }

        public string Url { get; set; }

        public string? Aciklama { get; set; }
    }

    public int Id { get; set; }

    public string DosyaNo { get; set; }

    public string BuroNo { get; set; }

    public string Konu { get; set; }

    public string Aciklama { get; set; }

    public string DosyaTuru { get; set; }

    public string DosyaKategorisi { get; set; }

    public string DosyaDurumu { get; set; }

    public string Mahkeme { get; set; } 

    public DateTime AcilisTarihi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public List<Taraf> Taraflar { get; set; } = new();

    public List<Personel> SorumluPersonel { get; set; } = new();

    public List<Baglanti> DosyaBaglantilari { get; set; } = new();

    public List<Durusma> Durusmalar { get; set; } = new();

    public List<Belge> Belgeler { get; set; } = new();
}
