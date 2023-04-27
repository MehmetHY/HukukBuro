namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class DosyaKategorisi
{
    public int Id { get; set; }

    public string Isim { get; set; }    // AsliyeHukuk | AylikHukukiDanismanlik | Icra | IdareMahkemesi | IsMahkemesi
}