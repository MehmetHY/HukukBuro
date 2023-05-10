namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class DosyaBaglantisiSilVM
{
    public int Id { get; set; }

    public int DosyaId { get; set; }

    public string IlgiliDosya { get; set; }

    public string? Aciklama { get; set; }
}
