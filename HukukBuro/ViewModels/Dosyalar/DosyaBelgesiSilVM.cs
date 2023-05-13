namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class DosyaBelgesiSilVM
{
    public int Id { get; set; }

    public int DosyaId { get; set; }

    public string Baslik { get; set; }

    public string? Aciklama { get; set; }

    public string Url { get; set; }

    public string Uzanti { get; set; }

    public string Boyut { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
}
