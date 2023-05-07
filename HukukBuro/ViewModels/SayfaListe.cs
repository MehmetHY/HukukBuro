namespace HukukBuro.ViewModels;

public class SayfaListe
{
    public int Sayfa { get; set; } = 1;
    public int SayfaBoyutu { get; set; } = Sabit.SayfaBoyutu;
    public int ToplamSayfa { get; set; } = 1;
    public bool SonrakiSayfaVarMi => ToplamSayfa > Sayfa;
    public bool OncekiSayfaVarMi => Sayfa > 1;
    public string? Arama { get; set; }
}

public class SayfaListe<T> : SayfaListe
{
    public List<T> Ogeler { get; set; } = new();
}
