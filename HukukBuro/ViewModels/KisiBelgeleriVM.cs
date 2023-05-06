namespace HukukBuro.ViewModels;

#pragma warning disable CS8618

public class KisiBelgeleriVM : SayfaListe<KisiBelgeleriVM.Oge>
{
    public int Id { get; set; }

    public class Oge
    {
        public int Id { get; set; }

        public string Baslik { get; set; }

        public string? Aciklama { get; set; }

        public string Url { get; set; }

        public bool OzelMi { get; set; }

        public string Uzanti { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public long Boyut { get; set; }

        public string OkunabilirBoyut => Boyut >= 1_000_000 ?
            $"{Boyut / 1_000_000.0:F1} MB" : $"{Boyut / 1_000} KB";
    }
}
