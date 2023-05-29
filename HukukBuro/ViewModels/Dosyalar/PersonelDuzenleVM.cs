namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class PersonelDuzenleVM
{
    public class Personel : CheckboxItem<string>
    {
        public string Anarol { get; set; }
    }

    public int Id { get; set; }

    public List<Personel> PersonelListe { get; set; } = new();

    public List<string> Anaroller { get; set; } = new();
}
