namespace HukukBuro.ViewModels;

public class KisiOzetVM
{
    public int Id { get; set; }

    public bool TuzelMi { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? SirketIsmi { get; set; }

    public string? VergiDairesi { get; set; }

    public string? VergiNo { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Adres { get; set; }

    public string? BankaHesapBilgisi { get; set; }

    public string? EkBilgi { get; set; }

    public int IlgiliKisiSayisi { get; set; }
    public int IlgiliDosyaSayisi { get; set; }
    public int RandevuSayisi { get; set; }
    public int GorevSayisi { get; set; }
    public int FinansSayisi { get; set; }
    public int BelgeSayisi { get; set; }
}
