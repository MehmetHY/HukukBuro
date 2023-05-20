namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class BildirimVM
{
    public int Id { get; set; }

    public string Mesaj { get; set; }

    public bool Okundu { get; set; }

    public string? Url { get; set; }

    public DateTime Tarih { get; set; }
}
