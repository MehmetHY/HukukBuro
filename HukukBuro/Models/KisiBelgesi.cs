namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class KisiBelgesi : Belge
{
    public int KisiId { get; set; }
    public Kisi Kisi { get; set; }

    public bool OzelMi { get; set; }
}
