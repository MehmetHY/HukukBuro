namespace HukukBuro;

public static class Sabit
{
    public const int SayfaBoyutu = 20;

    public static class GorevDurumu
    {
        public const string Tamamlandi = "Tamamlandı";
        public const string DevamEdiyor = "Devam Ediyor";
        public const string Iptal = "İptal";
    }

    public static class View
    {
        public const string Hata = "~/Views/Genel/Hata.cshtml";
    }

    public static class Belge
    {
        public static readonly string[] GecerliUzantilar = { ".JPG",
                                                             ".JPEG",
                                                             ".PNG",
                                                             ".PDF",
                                                             ".WEBP",
                                                             ".DOCX",
                                                             ".SVG",
                                                             ".TXT"};

        public const int MaxBoyut = 2_000_000;

        public const string HataMaxBoyut = "Belge 2 MB'tan büyük olamaz.";
        public const string HataGerekli = "Belge gerekli.";
    }
}
