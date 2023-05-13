namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class DosyaBelgesi : Belge
{
    public int DosyaId { get; set; }
    public Dosya Dosya { get; set; }
}
