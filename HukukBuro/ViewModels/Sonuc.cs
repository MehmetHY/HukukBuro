namespace HukukBuro.ViewModels;

public class Sonuc
{
    public bool BasariliMi { get; set; } = true;

    public string HataBasligi { get; set; } = string.Empty;

    public string HataMesaji { get; set; } = string.Empty;
}

public class Sonuc<T> : Sonuc
{
    public T? Deger { get; set; }
}

