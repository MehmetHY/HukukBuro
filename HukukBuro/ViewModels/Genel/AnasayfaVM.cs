namespace HukukBuro.ViewModels.Genel;

#pragma warning disable CS8618

public class AnasayfaVM
{
    public class Duyuru
    {
        public int Id { get; set; }

        public string Konu { get; set; }

        public string Mesaj { get; set; }

        public string? Url { get; set; }

        public DateTime Tarih { get; set; }
    }

    public List<Duyuru> Duyurular { get; set; } = new();

    public class Gorev
    {
        public int Id { get; set; }

        public string Konu { get; set; }

        public string? Aciklama { get; set; }

        public string BaglantiTuru { get; set; }

        public string Durum { get; set; }

        public int? KisiId { get; set; }
        public string? Kisi { get; set; }

        public int? DosyaId { get; set; }
        public string? Dosya { get; set; }

        public string? SorumluId { get; set; }
        public string? Sorumlu { get; set; }

        public DateTime BitisTarihi { get; set; }

        public DateTime OlusturmaTarihi { get; set; }
    }

    public List<Gorev> Gorevler { get; set; } = new();

    public class Randevu
    {
        public int Id { get; set; }

        public int KisiId { get; set; }
        public string Kisi { get; set; }

        public string Konu { get; set; }

        public string? Aciklama { get; set; }

        public DateTime Tarih { get; set; }

        public string? SorumluId { get; set; }
        public string? Sorumlu { get; set; }

        public bool TamamlandiMi { get; set; }
    }

    public List<Randevu> Randevular { get; set; } = new();

    public class Durusma
    {
        public int Id { get; set; }

        public int DosyaId { get; set; }
        public string Dosya { get; set; }

        public string AktiviteTuru { get; set; }

        public string? Aciklama { get; set; }

        public DateTime Tarih { get; set; }

        public bool Tamamlandi { get; set; }
    }

    public List<Durusma> Durusmalar { get; set; } = new();

    public class FinansIslemi
    {
        public int Id { get; set; }

        public string Miktar { get; set; }

        public bool Odendi { get; set; }
        public DateTime? SonOdemeTarhi { get; set; }
        public DateTime? OdemeTarhi { get; set; }
        public bool MakbuzKesildiMi { get; set; }
        public DateTime? MakbuzTarihi { get; set; }
        public string? MakbuzNo { get; set; }

        public string IslemTuru { get; set; }

        public int? KisiId { get; set; }
        public string? Kisi { get; set; }

        public int? DosyaId { get; set; }
        public string? Dosya { get; set; }

        public string? PersonelId { get; set; }
        public string? Personel { get; set; }

        public string? IslemYapanId { get; set; }
        public string? IslemYapan { get; set; }

        public string? Aciklama { get; set; }
    }

    public List<FinansIslemi> FinansIslemleri { get; set; } = new ();
}
