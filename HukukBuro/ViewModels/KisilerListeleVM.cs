namespace HukukBuro.ViewModels;

#pragma warning disable CS8618

public class KisilerListeleVM : SayfaListe<KisilerListeleVM.Oge>
{
    public class Oge
    {
        public int Id { get; set; }

        public string? Kisaltma { get; set; }

        public bool TuzelMi { get; set; }

        public string Isim { get; set; }

        public string? KimlikVergiNo { get; set; }

        public string? Telefon { get; set; }

        public string? Email { get; set; }

        public int DosyaSayisi { get; set; }
        public int RandevuSayisi { get; set; }
        public int GorevSayisi { get; set; }
        public int FinansSayisi { get; set; }
    }
}
