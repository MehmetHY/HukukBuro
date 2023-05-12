namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class DurusmaSilVM
{
    public int Id { get; set; }

    public int DosyaId { get; set; }

    public string AktiviteTuru { get; set; }

    public DateTime Tarih { get; set; }

    public string? Aciklama { get; set; }
}
