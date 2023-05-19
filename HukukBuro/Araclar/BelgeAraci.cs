using HukukBuro.ViewModels;

namespace HukukBuro.Araclar;

public class BelgeAraci
{
    public IFormFile? Belge { get; set; }
    public long MinBoyut { get; set; } = 0;
    public long MaxBoyut { get; set; } = Sabit.Belge.MaxBoyut;
    public string[] GecerliUzantilar { get; set; } = Sabit.Belge.GecerliBelgeUzantilari;

    public string? Root { get; set; }
    public string? Klasor { get; set; }
    public string? Isim { get; set; }
    public string? Uzanti { get; set; }
    public string? Url { get; set; }
    public string? Yol { get; set; }
    public long Boyut { get; set; }

    public bool UstuneYaz { get; set; }

    public Sonuc Onayla()
    {
        if (Belge == null)
            return new()
            {
                BasariliMi = false,
                HataMesaji = "Belge gerekli."
            };

        Boyut = Belge.Length;

        if (Boyut < MinBoyut)
            return new()
            {
                BasariliMi = false,
                HataMesaji = $"Belge boyutu {Yardimci.OkunabilirDosyaBoyutu(MinBoyut)}'tan küçük olamaz."
            };

        if (Boyut > MaxBoyut)
            return new()
            {
                BasariliMi = false,
                HataMesaji = $"Belge boyutu {Yardimci.OkunabilirDosyaBoyutu(MaxBoyut)}'tan büyük olamaz."
            };

        Uzanti = Path.GetExtension(Belge.FileName).ToLowerInvariant();

        if (GecerliUzantilar.Length > 0)
            if (string.IsNullOrWhiteSpace(Uzanti) || !GecerliUzantilar.Contains(Uzanti))
                return new()
                {
                    BasariliMi = false,
                    HataMesaji = $"Yüklenebilir uzantılar: '{string.Join("', '", GecerliUzantilar)}'"
                };

        return new();
    }

    public Sonuc Olustur()
    {
        if (Belge == null)
            return new()
            {
                BasariliMi = false,
                HataMesaji = "Belge gerekli."
            };

        if (string.IsNullOrWhiteSpace(Root))
            return new()
            {
                BasariliMi = false,
                HataMesaji = "Root belirtilmedi."
            };

        if (string.IsNullOrWhiteSpace(Isim))
            Isim = Guid.NewGuid().ToString();

        if (!string.IsNullOrWhiteSpace(Uzanti))
            Isim = Uzanti[0] == '.' ?
                $"{Isim}{Uzanti}" :
                $"{Isim}.{Uzanti}";

        var dir = string.IsNullOrWhiteSpace(Klasor) ?
            Root : Path.Combine(Root, Klasor);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        Yol = Path.Combine(dir, Isim);

        Url = string.IsNullOrWhiteSpace(Klasor) ?
            $"/{Isim}" : $"/{Klasor}/{Isim}";

        if (File.Exists(Yol))
        {
            if (!UstuneYaz)
                return new()
                {
                    BasariliMi = false,
                    HataMesaji = $"{Yol} zaten mevcut"
                };

            File.Delete(Yol);
        }

        var mod = UstuneYaz ? FileMode.Create : FileMode.CreateNew;

        using var stream = new FileStream(Yol, mod);
        Belge.CopyTo(stream);

        return new();
    }

    public Sonuc Sil()
    {
        if (string.IsNullOrWhiteSpace(Yol))
            return new()
            {
                BasariliMi = false,
                HataMesaji = "Belge yolu belirtilmedi."
            };

        if (File.Exists(Yol))
            File.Delete(Yol);

        return new();   
    }
}
