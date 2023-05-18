namespace HukukBuro.ViewModels.Duyurular;

#pragma warning disable CS8618

public class DuzenleVM
{
    public int Id { get; set; }

    public string Konu { get; set; }

    public string Mesaj { get; set; }

    public string? Url { get; set; }

    public bool BildirimGonder { get; set; }
}
