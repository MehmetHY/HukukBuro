namespace HukukBuro.ViewModels.Gorevler;

#pragma warning disable CS8618

public class ListeleVM : SayfaListe<ListeleVM.Oge>
{
    public class Oge
    {
        public int Id { get; set; }

        public BaglantiTuru BaglantiTuru { get; set; }

        public int? KisiId { get; set; }
        public string? KisiIsmi { get; set; }

        public int? DosyaId { get; set; }
        public string? DosyaIsmi { get; set; }

        public string? SorumluId { get; set; }
        public string? SorumluIsmi { get; set; }

        public string Konu { get; set; }

        public string? Aciklama { get; set; }
        
        public DateTime BitisTarihi { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public string Durum { get; set; }
    }
}
