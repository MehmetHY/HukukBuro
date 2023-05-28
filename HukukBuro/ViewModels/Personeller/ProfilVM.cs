using HukukBuro.Eklentiler;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class ProfilVM
{
    public string Id { get; set; }

    public string Isim { get; set; }

    public string Soyisim { get; set; }

    public string FotoUrl { get; set; }

    public string? Telefon { get; set; }

    public string Email { get; set; }

    public string Anarol { get; set; }

    public class FinansIslemi
    {
        public int Id { get; set; }

        public string Miktar { get; set; }

        public string IslemTuru { get; set; }

        public bool Odendi { get; set; }
        public DateTime? OdemeTarihi { get; set; }
        public DateTime? SonOdemeTarihi { get; set; }
        public bool MakbuzKesildi { get; set; }
        public DateTime? MakbuzTarihi { get; set; }
        public string? MakbuzNo { get; set; }

        public int? KisiId { get; set; }
        public string? Kisi { get; set; }

        public int? DosyaId { get; set; }
        public string? Dosya { get; set; }

        public string? PersonelId { get; set; }
        public string? Personel { get; set; }

        public string? Aciklama { get; set; }
    }

    public List<FinansIslemi> FinansIslemleri { get; set; } = new();

    public class Dosya
    {
        public int Id { get; set; }

        public string TamIsim { get; set; }

        public string DosyaDurumu { get; set; }

        public string DosyaTuru { get; set; }

        public string DosyaKategorisi { get; set; }
    }

    public List<Dosya> Dosyalar { get; set; } = new();

    public class Gorev
    {
        public int Id { get; set; }

        public string Konu { get; set; }

        public string Durum { get; set; }

        public string BaglantiTuru { get; set; }

        public int? KisiId { get; set; }
        public string? Kisi { get; set; }

        public int? DosyaId { get; set; }
        public string? Dosya { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public DateTime BitisTarihi { get; set; }

        public string? Aciklama { get; set; }
    }

    public List<Gorev> Gorevler { get; set; } = new();

    public class Randevu
    {
        public int Id { get; set; }

        public int KisiId { get; set; }
        public string Kisi { get; set; }

        public string Konu { get; set; }

        public bool Tamamlandi { get; set; }

        public DateTime Tarih { get; set; }

        public string? Aciklama { get; set; }
    }

    public List<Randevu> Randevular { get; set; } = new();
}
