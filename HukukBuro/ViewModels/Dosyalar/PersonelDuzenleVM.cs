namespace HukukBuro.ViewModels.Dosyalar;

#pragma warning disable CS8618

public class PersonelDuzenleVM
{
    public int Id { get; set; }

    public List<CheckboxItem<string>> PersonelListe { get; set; } = new();
}
