using HukukBuro.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    public static class AnaRol
    {
        public const string Type = "AnaRol";

        public const string Admin = "Admin";
        public const string Yonetici = "Yönetici";
        public const string Avukat = "Avukat";
        public const string Calisan = "Çalışan";
    }

    public static class Yetki
    {
        public const string Kisi = "KisiYoneticisi";
        public const string Dosya = "DosyaYoneticisi";
        public const string Personel = "PersonelYoneticisi";
        public const string Gorev = "GorevYoneticisi";
        public const string Duyuru = "DuyuruYoneticisi";
        public const string Rol = "RolYoneticisi";

        public const string KisiText = "Kişileri düzenleyebilir.";
        public const string DosyaText = "Dosyaları düzenleyebilir.";
        public const string PersonelText = "Personeli düzenleyebilir.";
        public const string GorevText = "Görevleri düzenleyebilir.";
        public const string DuyuruText = "Duyuruları düzenleyebilir.";
        public const string RolText = "Rol ve yetkileri düzenleyebilir.";
    }
}
