namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class SilVM
{
    public int Id { get; set; }

    public string DosyaNo { get; set; }

    public string BuroNo { get; set; }

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public string DosyaTuru { get; set; }

    public string DosyaKategorisi { get; set; }

    public string DosyaDurumu { get; set; }

    public string? Mahkeme { get; set; }

    public DateTime AcilisTarihi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
}
