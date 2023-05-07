namespace HukukBuro.ViewModels.Kisiler;

public class KisiSilVM
{
    public int Id { get; set; }

    public bool TuzelMi { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? SirketIsmi { get; set; }

    public int IlgiliKisiSayisi { get; set; }
    public int IlgiliDosyaSayisi { get; set; }
    public int RandevuSayisi { get; set; }
    public int GorevSayisi { get; set; }
    public int FinansSayisi { get; set; }
    public int VekaletnameSayisi { get; set; }
    public int BelgeSayisi { get; set; }
}
