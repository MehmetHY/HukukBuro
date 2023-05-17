namespace HukukBuro.ViewModels.FinansIslemleri;

#pragma warning disable CS8618

public class ListeleVM : SayfaListe<ListeleVM.Oge>
{
    public class Oge
    {
        public int Id { get; set; }

        public string Miktar { get; set; }

        public IslemTuru IslemTuru { get; set; }

        public bool Odendi { get; set; }
        public DateTime? OdemeTarihi { get; set; }
        public DateTime? SonOdemeTarihi { get; set; }
        public bool MakbuzKesildi { get; set; }
        public DateTime? MakbuzTarihi { get; set; }
        public string? MakbuzNo { get; set; }

        public string? IslemYapan { get; set; }
        public string? Kisi { get; set; }
        public string? Dosya { get; set; }
        public string? Personel { get; set; }   

        public string? Aciklama { get; set; }
    }
}
