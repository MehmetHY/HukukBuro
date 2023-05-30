using HukukBuro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HukukBuro.Data;

public class VeriBaslatici
{
    private readonly VeriTabani _veriTabani;
    private readonly UserManager<Personel> _kullaniciYoneticisi;
    private readonly IConfiguration _config;

    public VeriBaslatici(
        VeriTabani veriTabani,
        UserManager<Personel> kullaniciYoneticisi,
        IConfiguration config)
    {
        _veriTabani = veriTabani;
        _kullaniciYoneticisi = kullaniciYoneticisi;
        _config = config;
    }

    public void BaslangicVerileriniKaydet()
    {
        if (_veriTabani.Database.GetPendingMigrations().Count() < 1)
            return;

        _veriTabani.Database.Migrate();
        AdminVerileriniGir().GetAwaiter().GetResult();
        PersonelVerileriniGir().GetAwaiter().GetResult();
        KisiVerileriniGir();
        DosyaVerileriniGir();
        DuyuruVerileriniGir();
        GorevVerileriniGir();
        RandevuVerileriniGir();
        DurusmaVerileriniGir();
        FinansIslemiVerileriniGir();
    }

    private async Task AdminVerileriniGir()
    {
        var adminVar = _veriTabani.UserClaims
            .Any(uc =>
                uc.ClaimType == Sabit.AnaRol.Type &&
                uc.ClaimValue == Sabit.AnaRol.Admin);

        if (adminVar)
            return;

        var adminIsmi = _config["AdminIsmi"] ??
            throw new KeyNotFoundException("Admin ismini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminIsmi)");

        var adminSoyismi = _config["AdminSoyismi"] ??
            throw new KeyNotFoundException("Admin soyismini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminSoyismi)");

        var adminEmail = _config["AdminEmail"] ??
            throw new KeyNotFoundException("Admin emailini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminEmail)");

        var adminSifre = _config["AdminSifre"] ??
            throw new KeyNotFoundException("Admin şifresini çevre değişkenlerine ya da appsettings.json'a ekle ekle. (key: AdminSifre)");

        var admin = new Personel
        {
            UserName = adminEmail,
            Email = adminEmail,
            Isim = adminIsmi,
            Soyisim = adminSoyismi,
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        var result = await _kullaniciYoneticisi.CreateAsync(admin, adminSifre);

        if (!result.Succeeded)
            throw new InvalidDataException(
                $"Admin hesabı oluştururken bir hata meydana geldi. {string.Join(",", result.Errors.Select(e => e.Description).ToList())}");

        result = await _kullaniciYoneticisi.AddClaimAsync(admin, new Claim(Sabit.AnaRol.Type, Sabit.AnaRol.Admin));

        if (!result.Succeeded)
            throw new Exception(
                $"Admine anarol verirken bir hata meydana geldi. {string.Join(",", result.Errors.Select(e => e.Description).ToList())}");
    }

    private async Task PersonelVerileriniGir()
    {
        var sifre = _config["OrnekKullaniciSifre"]
            ?? throw new KeyNotFoundException("'OrnekKullaniciSifre' appsettings.json'da bulunamadı.");

        var ahmet = new Personel
        {
            Email = "ahmet@test.com",
            UserName = "ahmet@test.com",
            Isim = "Ahmet",
            Soyisim = "YILMAZ",
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl,
            PhoneNumber = "+90 500 000 00 01"
        };

        await _kullaniciYoneticisi.CreateAsync(ahmet, sifre);

        await _kullaniciYoneticisi.AddClaimAsync(
            ahmet,
            new Claim(Sabit.AnaRol.Type, Sabit.AnaRol.Yonetici));

        await _kullaniciYoneticisi.AddToRolesAsync(
            ahmet,
            new List<string>
            {
                Sabit.Yetki.Kisi,
                Sabit.Yetki.Dosya,
                Sabit.Yetki.Gorev,
                Sabit.Yetki.Finans,
                Sabit.Yetki.Randevu
            });


        var ayse = new Personel
        {
            Email = "ayse@test.com",
            UserName = "ayse@test.com",
            Isim = "Ayşe",
            Soyisim = "YILDIRIM",
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl,
            PhoneNumber = "+90 500 000 00 02"
        };

        await _kullaniciYoneticisi.CreateAsync(ayse, sifre);

        await _kullaniciYoneticisi.AddClaimAsync(
            ayse,
            new Claim(Sabit.AnaRol.Type, Sabit.AnaRol.Avukat));


        var huseyin = new Personel
        {
            Email = "huseyin@test.com",
            UserName = "huseyin@test.com",
            Isim = "Hüseyin",
            Soyisim = "BOZKURT",
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl,
            PhoneNumber = "+90 500 000 00 03"
        };

        await _kullaniciYoneticisi.CreateAsync(huseyin, sifre);

        await _kullaniciYoneticisi.AddClaimAsync(
            huseyin,
            new Claim(Sabit.AnaRol.Type, Sabit.AnaRol.Avukat));


        var burak = new Personel
        {
            Email = "burak@test.com",
            UserName = "burak@test.com",
            Isim = "Burak",
            Soyisim = "AKTÜRK",
            FotoUrl = Sabit.Belge.VarsayilanFotoUrl,
            PhoneNumber = "+90 500 000 00 04"
        };

        await _kullaniciYoneticisi.CreateAsync(burak, sifre);

        await _kullaniciYoneticisi.AddClaimAsync(
            burak,
            new Claim(Sabit.AnaRol.Type, Sabit.AnaRol.Calisan));
    }

    private void KisiVerileriniGir()
    {
        var hakan = new Kisi
        {
            TuzelMi = false,
            Isim = "Hakan",
            Soyisim = "AYYILDIZ",
            Email = "hakan@test.com",
            Telefon = "+90 500 000 00 05",
            Kisaltma = "HA",
            KimlikNo = "00000000005",
            AdresBilgisi = "Çilek Mah. Kırmızı Sokak. No: 5 Daire:6",
            BankaHesapBilgisi = "TR000000000000000000000005",
            EkBilgi = "İşlemlerde 25% indirim yapılacak."
        };


        var ozgur = new Kisi
        {
            TuzelMi = false,
            Isim = "Özgür",
            Soyisim = "YURT",
            Email = "ozgur@test.com",
            Telefon = "+90 500 000 00 06",
            Kisaltma = "ÖY",
            KimlikNo = "00000000006",
            AdresBilgisi = "Muz Mah. Sarı Sokak. No: 6",
            BankaHesapBilgisi = "TR000000000000000000000006"
        };

        var burcu = new Kisi
        {
            TuzelMi = false,
            Isim = "Burcu",
            Soyisim = "ÖZYİĞİT",
            Email = "burcu@test.com",
            Telefon = "+90 500 000 00 07",
            Kisaltma = "BÖ",
            KimlikNo = "00000000007",
            AdresBilgisi = "Kiraz Mah. Kızıl Sokak. No: 7",
            BankaHesapBilgisi = "TR000000000000000000000007"
        };

        var hilmi = new Kisi
        {
            TuzelMi = false,
            Isim = "Hilmi",
            Soyisim = "ÖZYİĞİT",
            Email = "hilmi@test.com",
            Telefon = "+90 500 000 00 09",
            Kisaltma = "HÖ",
            KimlikNo = "00000000009",
            AdresBilgisi = "Kiraz Mah. Kızıl Sokak. No: 9",
            BankaHesapBilgisi = "TR000000000000000000000009"
        };


        var yilmazlar = new Kisi
        {
            TuzelMi = true,
            SirketIsmi = "YILMAZLAR A.Ş.",
            VergiDairesi = "Başiskele Mal Müdürlüğü",
            VergiNo = "98200008",
            Email = "yilmazlar@test.com",
            Telefon = "+90 500 000 00 08",
            Kisaltma = "YI",
            KimlikNo = "00000000008",
            AdresBilgisi = "Erik Mah. Yeşil Sokak. No: 8",
            BankaHesapBilgisi = "TR000000000000000000000008"
        };

        _veriTabani.Kisiler.AddRange(hakan, ozgur, burcu, hilmi, yilmazlar);
        _veriTabani.SaveChanges();

        var baglantilar = new List<KisiBaglantisi>
        {
            new()
            {
                KisiId = hakan.Id,
                IlgiliKisiId = yilmazlar.Id,
                Pozisyon = "Çalıştığı iş yeri",
                
            },

            new()
            {
                KisiId = yilmazlar.Id,
                IlgiliKisiId = hakan.Id,
                Pozisyon = "CEO"
            },

            new()
            {
                KisiId = burcu.Id,
                IlgiliKisiId = hilmi.Id,
                Pozisyon = "Kocası"
            },

            new()
            {
                KisiId = hilmi.Id,
                IlgiliKisiId = burcu.Id,
                Pozisyon = "Karısı"
            },
        };

        _veriTabani.KisiBaglantilari.AddRange(baglantilar);
        _veriTabani.SaveChanges();
    }

    private void DosyaVerileriniGir()
    {
        var miras = new Dosya
        {
            Konu = "Miras",
            DosyaNo = "001",
            BuroNo = "2/B",
            Aciklama = "6704 Sayılı torba Yasa ile 2918 Sayılı Karayolları Trafik Kanunu’nun 90-92-97-99. Maddelerinde değişiklik yapılmış, 4925 Sayılı Karayolu Taşıma Kanunu’nun ise 17-18-19-20-21-22-23-24-25-26/1/ı-i maddeleri tamamen yürürlükten kaldırılarak, tazminat hukukunda, tazminatın belirlenmesindeki  usul ve esaslar önemli ve dikkate değer ölçüde değişmiş, Zorunlu Mali Sorumluluk Sigortasının poliçe teminatı kapsamında da, yine sigortalılar ve yakınları aleyhine ciddi daralmalar yapılmıştır. Yapılan düzenlemelerin tamamen sigorta şirketleri lehine yapılmış olması bir yana, uzun yıllardır süregelen, Yargıtay içtihadları ile yerleşmiş hesaplama teknikleri ve ilke kararları bir tarafa bırakılmış, tüm yetki ve insiyatif adeta Hazine Müsteşarlığı eliyle sigorta şirketlerine bırakılmıştır.",
            AcilisTarihi = DateTime.Now.AddYears(-3).AddMonths(-5).AddDays(-12),
            BolgeAdliyeMahkemesiBilgileri = new(),
            DosyaDurumuId = 3,
            DosyaKategorisiId = 1,
            DosyaTuruId = 1,
            KararBilgileri = new(),
            KararDuzeltmeBilgileri = new(),
            TemyizBilgileri = new(),
            OlusturmaTarihi = DateTime.Now.AddYears(-3).AddMonths(-5).AddDays(-11),
            Mahkeme = "Kocaeli Adliyesi",
            KesinlesmeBilgileri = new()
        };

        var hakaret = new Dosya
        {
            Konu = "Sosyal Medya Hakaret Davası",
            DosyaNo = "002",
            BuroNo = "3/A",
            Aciklama = "iş yerinde hedef alınan kişi veya kişilere yönelik belirli süre devam eden; onların çalışma konusunda motivasyonunu kıran, psikolojik olarak yıpratan kasıtlı ya da kötü niyetli davranışlar mobbing kapsamında girmektedir",
            AcilisTarihi = DateTime.Now.AddMonths(-2).AddDays(-2),
            BolgeAdliyeMahkemesiBilgileri = new(),
            DosyaDurumuId = 7,
            DosyaKategorisiId = 2,
            DosyaTuruId = 2,
            KararBilgileri = new(),
            KararDuzeltmeBilgileri = new(),
            TemyizBilgileri = new(),
            OlusturmaTarihi = DateTime.Now.AddMonths(-3).AddDays(-2),
            Mahkeme = "Bursa Adliyesi",
            KesinlesmeBilgileri = new()
        };

        _veriTabani.Dosyalar.AddRange(miras, hakaret);
        _veriTabani.SaveChanges();
    }

    private void DuyuruVerileriniGir()
    {
        var duyuru1 = new Duyuru
        {
            Konu = "Yeni Güncelleme",
            Mesaj = "Randevu sistemi güncellendi",
            Url = "/randevular/listele",
            Tarih = DateTime.Now.AddMonths(-8)
        };

        var duyuru2 = new Duyuru
        {
            Konu = "Yeni Atamalar",
            Mesaj = "Dosya ve finans işlemi sorumlulukları güncellendi. Lütfen kontrol ediniz.",
            Url = "/personel/profil",
            Tarih = DateTime.Now.AddMonths(-9)
        };

        _veriTabani.Duyurular.AddRange(duyuru1, duyuru2);
        _veriTabani.SaveChanges();
    }

    private void GorevVerileriniGir()
    {
        var gorevler = new List<Gorev>
        {
            new()
            {
                Konu = "Dosyayı Kapat",
                Aciklama = "Dosyayı en kısa sürede kapat.",
                BaglantiTuru = 1,
                BitisTarihi = DateTime.Now.AddDays(6),
                DosyaId = 1,
                DurumId = 2,
                OlusturmaTarihi = DateTime.Now.AddDays(-4)
            },

            new()
            {
                Konu = "Tarihlerin Güncellenmesi",
                Aciklama = "Önümüzdeki resmi tatil nedeniyle; görev ve randevuların tarihlerini güncelle",
                BaglantiTuru = 0,
                BitisTarihi = DateTime.Now.AddDays(9),
                DurumId = 1,
                OlusturmaTarihi = DateTime.Now.AddDays(-9)
            },

            new()
            {
                Konu = "Dosyaların Kontrolü",
                Aciklama = "Gerek kalmadığı için iptal edildi.",
                BaglantiTuru = 0,
                BitisTarihi = DateTime.Now.AddDays(-3),
                DurumId = 3,
                OlusturmaTarihi = DateTime.Now.AddDays(-12)
            },
        };

        _veriTabani.Gorevler.AddRange(gorevler);
        _veriTabani.SaveChanges();
    }

    private void RandevuVerileriniGir()
    {
        var randevular = new List<Randevu>
        {
            new()
            {
                KisiId = 1,
                Konu = "Alacak ile ilgili",
                Aciklama = "Dosya masraflarının ödenmesi hakkında",
                OlusturmaTarihi = DateTime.Now.AddDays(-43),
                Tarih = DateTime.Now.AddDays(12),
                TamamlandiMi = false
            },

            new()
            {
                KisiId = 2,
                Konu = "Duruşma Bilgilendirme",
                Aciklama = "Önümüzdeki duruşma ile ilgili, müvekkile gerekli bilgilendirmenin yapılması.",
                OlusturmaTarihi = DateTime.Now.AddDays(-83),
                Tarih = DateTime.Now.AddDays(-4),
                TamamlandiMi = true
            }
        };

        _veriTabani.Randevular.AddRange(randevular);
        _veriTabani.SaveChanges();
    }

    private void DurusmaVerileriniGir()
    {
        var durusmalar = new List<Durusma>
        {
            new()
            {
                AktiviteTuruId = 1,
                DosyaId = 1,
                Tamamlandi = false,
                Tarih = DateTime.Now.AddDays(123),
                Aciklama = "Miras bırakanın borçları, destek sağladığı veya sağlamakla yükümlü olduğu kişilere bağlanacak aylıklar, cenaze masrafları vb. giderler de dikkate alınmak suretiyle hesaplama yapıldı."
            },

            new()
            {
                AktiviteTuruId = 2,
                DosyaId = 2,
                Tamamlandi = true,
                Tarih = DateTime.Now.AddDays(-5),
                Aciklama = "Dava, ikinci konuşmaya ilişkin ses kayıtlarının hükme dayanak yapılıp yapılamayacağına dairdir. Kişilerin ses kayıtlarının kişisel veri olduğundan şüphe yoktur."
            },
        };

        _veriTabani.Durusmalar.AddRange(durusmalar);
        _veriTabani.SaveChanges();
    }

    private void FinansIslemiVerileriniGir()
    {
        var islemler = new List<FinansIslemi>
        {
            new()
            {
                Aciklama = "Ofis temizlik ücreti yatırıldı.",
                IslemTuru = 3,
                MakbuzKesildiMi = true,
                MakbuzNo = "0010546",
                MakbuzTarihi = DateTime.Now.AddMonths(-3).AddDays(1),
                Odendi = true,
                OdemeTarhi = DateTime.Now.AddMonths(-3).AddDays(1),
                Miktar = 654.95M,
                OlusturmaTarihi = DateTime.Now.AddMonths(-3).AddDays(12),
            },

            new()
            {
                Aciklama = "Duruşma masrafları ödenecek.",
                IslemTuru = 7,
                SonOdemeTarhi = DateTime.Now.AddMonths(1).AddDays(14),
                Miktar = 1200M,
                OlusturmaTarihi = DateTime.Now.AddDays(-9),
                DosyaBaglantisiVar = true,
                DosyaId = 2
            }
        };

        _veriTabani.FinansIslemleri.AddRange(islemler);
        _veriTabani.SaveChanges();
    }
}
