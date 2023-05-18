namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Bildirim
{
    public int Id { get; set; }

    public string Mesaj { get; set; }

    public string PersonelId { get; set; }
    public Personel Personel { get; set; }

    public bool Okundu { get; set; }

    public string? Url { get; set; }

    public DateTime Tarih { get; set; }
}
