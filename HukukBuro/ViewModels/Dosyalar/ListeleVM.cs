namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class ListeleVM : SayfaListe<ListeleVM.Oge>
{
    public class Oge
    {
        public int Id { get; set; }

        public string DosyaNo { get; set; }

        public string BuroNo { get; set; }

        public string Konu { get; set; }

        public string DosyaTuru { get; set; }

        public string DosyaKategorisi { get; set; }

        public string DosyaDurumu { get; set; }

        public DateTime AcilisTarihi { get; set; }

        public int GorevSayisi { get; set; }

        public int DurusmaSayisi { get; set; }

        public int BelgeSayisi { get; set; }

        public int FinansSayisi { get; set; }
    }
}
