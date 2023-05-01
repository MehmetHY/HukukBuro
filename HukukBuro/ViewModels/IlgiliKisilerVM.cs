namespace HukukBuro.ViewModels;

#pragma warning disable CS8618

public class IlgiliKisilerVM : SayfaListe<IlgiliKisilerVM.Oge>
{
    public int Id { get; set; }

    public class Oge
    {
        public int Id { get; set; }

        public string Isim { get; set; }

        public string? Pozisyon { get; set; }
    }
}
