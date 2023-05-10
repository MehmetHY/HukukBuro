namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class ListeleVM : SayfaListe<ListeleVM.Oge>
{
    public class Oge
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string TamIsim { get; set; }

        public string Anarol { get; set; }
    }
}
