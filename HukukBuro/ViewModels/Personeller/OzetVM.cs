namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class OzetVM
{
    public string Id { get; set; }

    public string TamIsim { get; set; }

    public string Anarol { get; set; }

    public string Email { get; set; }

    public string FotoUrl { get; set; }

    public int IlgiliFinansIslemiSayisi { get; set; }
    public int SorumluFinansIslemiSayisi { get; set; }
    public int SorumluDosyaSayisi { get; set; }
    public int SorumluGorevSayisi { get; set; }
    public int SorumluRandevuSayisi { get; set; }
}
