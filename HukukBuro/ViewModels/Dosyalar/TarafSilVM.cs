namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class TarafSilVM
{
    public int Id { get; set; }

    public int DosyaId { get; set; }

    public string Isim { get; set; }

    public bool KarsiTarafMi { get; set; }

    public string TarafTuru { get; set; }
}
