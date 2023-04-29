namespace HukukBuro.ViewModels;

public class SayfaListe<T>
{
    public List<T> Ogeler { get; set; } = new();
    public int Sayfa { get; set; } = 1;
    public int ToplamSayfa { get; set; } = 1;
    public bool SonrakiSayfaVarMi => ToplamSayfa > Sayfa;
    public bool OncekiSayfaVarMi => Sayfa > 1;
}
