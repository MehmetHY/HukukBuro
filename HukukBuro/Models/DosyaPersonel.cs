namespace HukukBuro.Models;

#pragma warning disable CS8618

public class DosyaPersonel
{
    public int Id { get; set; }

    public int DosyaId { get; set; }
    public Dosya Dosya { get; set; }

    public string PersonelId { get; set; }
    public Personel Personel { get; set; }
}
