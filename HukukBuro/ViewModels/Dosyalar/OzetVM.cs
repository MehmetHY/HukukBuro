namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class OzetVM
{
    public class Taraf
    {
        public int Id { get; set; }

        public string Isim { get; set; }

        public string TarafTuru { get; set; }
    }

    public class Personel
    {
        public string TamIsim { get; set; }

        public string AnaRol { get; set; }
    }

    public int Id { get; set; }

    public int DosyaNo { get; set; }

    public string BuroNo { get; set; }

    public string Konu { get; set; }

    public string Aciklama { get; set; }

    public string DosyaTuru { get; set; }

    public string DosyaKategorisi { get; set; }

    public string DosyaDurumu { get; set; }

    public string Mahkeme { get; set; } 

    public DateTime AcilisTarihi { get; set; }

    public List<Taraf> MuvekkilTaraf { get; set; } = new();
    public List<Taraf> KarsiTaraf { get; set; } = new();

    public List<Personel> SorumluPersonel { get; set; } = new();
}
