namespace HukukBuro.Models;

#pragma warning disable CS8618

public class DosyaDurumu
{
    public int Id { get; set; }

    public string Isim { get; set; }    // Acik | Arsiv | Derdest | Hazirlik | Istinaf | Kapali(Aciz Vesikasi) | Kapali(Infaz) | Karar | Temyiz
}