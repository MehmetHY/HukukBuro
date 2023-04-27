namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Vekaletname : Belge
{
    public string NoterAdi { get; set; }

    public Kisi Kisi { get; set; }

    public DateTime NoterTarihi { get; set; }

    public string YevmiyeNo { get; set; }
}