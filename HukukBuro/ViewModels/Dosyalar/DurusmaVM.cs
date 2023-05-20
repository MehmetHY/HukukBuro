namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class DurusmaVM
{
    public int Id { get; set; }

    public int DosyaId { get; set; }

    public string DosyaTamisim { get; set; }

    public string AktiviteTuru { get; set; }

    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; }

    public bool Tamamlandi { get; set; }
}
