using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Personeller;

#pragma warning disable CS8618

public class YetkiDuzenleVM
{
    public string Id { get; set; }

    public string Anarol { get; set; }
    public List<SelectListItem> Anaroller { get; set; } = new();

    public List<CheckboxItem<string>> Yetkiler { get; set; } = new();
}
