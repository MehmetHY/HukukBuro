﻿namespace HukukBuro;

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
        public const string Bilgi = "~/Views/Genel/Bilgi.cshtml";
    }

    public static class Belge
    {
        public static readonly string[] GecerliBelgeUzantilari = { ".jpg",
                                                                   ".jpeg",
                                                                   ".png",
                                                                   ".gif",
                                                                   ".pdf",
                                                                   ".webp",
                                                                   ".docx",
                                                                   ".svg",
                                                                   ".txt"};

        public static readonly string[] GecerliFotoUzantilari = { ".jpg",
                                                                  ".jpeg",
                                                                  ".png",
                                                                  ".webp",
                                                                  ".gif",
                                                                  ".svg"};

        public const int MaxBoyut = 2_000_000;

        public const string HataMaxBoyut = "Belge 2 MB'tan büyük olamaz.";
        public const string HataGerekli = "Belge gerekli.";

        public const string VarsayilanFotoUrl = "/foto/kisi.svg";
    }

    public static class AnaRol
    {
        public const string Type = "AnaRol";

        public const string Admin = "Admin";
        public const string Yonetici = "Yönetici";
        public const string Avukat = "Avukat";
        public const string Calisan = "Çalışan";
        public const string Onaylanmamis = "Onaylanmamış";
    }

    public static class Yetki
    {
        public const string Kisi = "KisiYoneticisi";
        public const string Dosya = "DosyaYoneticisi";
        public const string Personel = "PersonelYoneticisi";
        public const string Gorev = "GorevYoneticisi";
        public const string Duyuru = "DuyuruYoneticisi";
        public const string Rol = "RolYoneticisi";
        public const string Finans = "FinansYoneticisi";
        public const string Randevu = "RandevuYoneticisi";

        public const string KisiText = "Kişileri düzenleyebilir.";
        public const string DosyaText = "Dosyaları düzenleyebilir.";
        public const string PersonelText = "Personeli düzenleyebilir.";
        public const string GorevText = "Görevleri düzenleyebilir.";
        public const string DuyuruText = "Duyuruları düzenleyebilir.";
        public const string RolText = "Rol ve yetkileri düzenleyebilir.";
        public const string FinansText = "Finans işlemlerini düzenleyebilir.";
        public const string RandevuText = "Randevu düzenleyebilir.";
    }

    public static class Policy
    {
        public const string Dosya = nameof(Dosya);
        public const string Duyuru = nameof(Duyuru);
        public const string Finans = nameof(Finans);
        public const string Gorev = nameof(Gorev);
        public const string Kisi = nameof(Kisi);
        public const string Personel = nameof(Personel);
        public const string Randevu = nameof(Randevu);
        public const string Rol = nameof(Rol);
    }

    public static class DosyaTuru
    {
        public const int DavaId = 1;
        public const int DanismanlikId = 2;
        public const int IcraId = 3;
        public const int ArabuluculukId = 4;
        public const int SorusturmaId = 5;

        public const string Dava = "Dava";
        public const string Danismanlik = "Danışmanlık";
        public const string Icra = "İcra";
        public const string Arabuluculuk = "Arabuluculuk";
        public const string Sorusturma = "Soruşturma";
    }

    public static class Regex
    {
        public const string Para = @"^\-?\s*\d+[,|\.]?(?:\d{0,2})?\s*$";
    }

    public static class Mesaj
    {
        public const string YeniDuyuru = "Yeni bir duyuru eklendi.";
    }

    public static class Hata
    {
        public const string Gerekli = nameof(Gerekli);
        public const string Para = "Örnek format: 5000,99";
    }
}
