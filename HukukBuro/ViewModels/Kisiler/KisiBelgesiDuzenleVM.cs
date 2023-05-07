namespace HukukBuro.ViewModels.Kisiler;

public class KisiBelgesiDuzenleVM
{
    public int Id { get; set; }

    public int KisiId { get; set; }

    public string Baslik { get; set; }

    public string? Aciklama { get; set; }

    public bool OzelMi { get; set; }

    public bool BelgeyiDegistir { get; set; }
}
