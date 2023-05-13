namespace HukukBuro.ViewModels.Dosyalar;

public class DosyaBelgesiDuzenleVM
{
    public int Id { get; set; }

    public int DosyaId { get; set; }

    public string Baslik { get; set; }

    public string? Aciklama { get; set; }

    public bool BelgeyiDegistir { get; set; }
}
