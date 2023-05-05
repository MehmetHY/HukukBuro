using HukukBuro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Data;

public sealed class VeriTabani : IdentityDbContext<Personel>
{
    public DbSet<Duyuru> Duyurular { get; set; }
    public DbSet<DuyuruKategorisi> DuyuruKategorileri { get; set; }
    public DbSet<BolgeAdliyeMahkemesiBilgileri> BolgeAdliyeMahkemesiBilgileri { get; set; }
    public DbSet<Dosya> Dosyalar { get; set; }
    public DbSet<DosyaBaglantisi> DosyaBaglantilari { get; set; }
    public DbSet<DosyaBelgesi> DosyaBelgeleri { get; set; }
    public DbSet<DosyaDurumu> DosyaDurumu { get; set; }
    public DbSet<DosyaFinansIslemi> DosyaFinansIslemleri { get; set; }
    public DbSet<DosyaGorevi> DosyaGorevleri { get; set; }
    public DbSet<DosyaKategorisi> DosyaKategorileri { get; set; }
    public DbSet<DosyaTuru> DosyaTurleri { get; set; }
    public DbSet<Durusma> Durusmalar { get; set; }
    public DbSet<DurusmaAktiviteTuru> DurusmaAktiviteTurleri { get; set; }
    public DbSet<FinansIslemTuru> FinansIslemTurleri { get; set; }
    public DbSet<KisiGorevi> KisiGorevleri { get; set; }
    public DbSet<GorevDurumu> GorevDurumlari { get; set; }
    public DbSet<KararBilgileri> KararBilgileri { get; set; }
    public DbSet<KararDuzeltmeBilgileri> KararDuzeltmeBilgileri { get; set; }
    public DbSet<KesinlesmeBilgileri> KesinlesmeBilgileri { get; set; }
    public DbSet<Kisi> Kisiler { get; set; }
    public DbSet<KisiBaglantisi> KisiBaglantilari { get; set; }
    public DbSet<KisiBelgesi> KisiBelgeleri { get; set; }
    public DbSet<KisiFinansIslemi> KisiFinansIslemleri { get; set; }
    public DbSet<Ofis> Ofis { get; set; }
    public DbSet<PersonelFinansIslemi> PersonelFinansIslemleri { get; set; }
    public DbSet<Randevu> Randevular { get; set; }
    public DbSet<TarafKisi> TarafKisiler { get; set; }
    public DbSet<TarafTuru> TarafTurleri { get; set; }
    public DbSet<TemyizBilgileri> TemyizBilgileri { get; set; }

    public VeriTabani(DbContextOptions<VeriTabani> ayarlar) : base(ayarlar)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Dosya>().Navigation(d => d.DosyaTuru).AutoInclude();
        builder.Entity<Dosya>().Navigation(d => d.DosyaDurumu).AutoInclude();
        builder.Entity<Dosya>().Navigation(d => d.DosyaKategorisi).AutoInclude();

        builder.Entity<Dosya>()
            .HasOne(d => d.DosyaTuru)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Dosya>()
            .HasOne(d => d.DosyaDurumu)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Dosya>()
            .HasOne(d => d.DosyaKategorisi)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<DosyaBaglantisi>()
            .HasOne(d => d.IlgiliDosya)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Durusma>()
            .HasOne(d => d.AktiviteTuru)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Duyuru>()
            .HasOne(d => d.Kategori)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<TarafKisi>()
            .HasOne(d => d.TarafTuru)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<KisiGorevi>()
            .HasOne(d => d.Durum)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<DosyaGorevi>()
            .HasOne(d => d.Durum)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<DosyaFinansIslemi>()
            .HasOne(d => d.IslemTuru)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<KisiFinansIslemi>()
            .HasOne(d => d.IslemTuru)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<PersonelFinansIslemi>()
            .HasOne(d => d.IslemTuru)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<DurusmaAktiviteTuru>()
            .HasData
            (
                new DurusmaAktiviteTuru { Id = 1, Isim = "Duruşma" },
                new DurusmaAktiviteTuru { Id = 2, Isim = "Keşif" },
                new DurusmaAktiviteTuru { Id = 3, Isim = "İnceleme" }
            );


        builder.Entity<TarafTuru>()
            .HasData
            (
                new TarafTuru { Id = 1, Isim = "Davacı" },
                new TarafTuru { Id = 2, Isim = "Davalı" },
                new TarafTuru { Id = 3, Isim = "Diğer" },
                new TarafTuru { Id = 4, Isim = "Avukat" },
                new TarafTuru { Id = 5, Isim = "Alacaklı" },
                new TarafTuru { Id = 6, Isim = "Borçlu" }

            );


        builder.Entity<DosyaTuru>()
            .HasData
            (
                new DosyaTuru { Id = 1, Isim = "Dava" },
                new DosyaTuru { Id = 2, Isim = "Danışmanlık" },
                new DosyaTuru { Id = 3, Isim = "İcra" },
                new DosyaTuru { Id = 4, Isim = "Arabuluculuk" },
                new DosyaTuru { Id = 5, Isim = "Soruşturma" }
            );


        builder.Entity<DosyaKategorisi>()
            .HasData
            (
                new DosyaKategorisi { Id = 1, Isim = "Asliye Hukuk" },
                new DosyaKategorisi { Id = 2, Isim = "Aylık Hukuk Danışmanlık" },
                new DosyaKategorisi { Id = 3, Isim = "İcra" },
                new DosyaKategorisi { Id = 4, Isim = "İdare Mahkemesi" },
                new DosyaKategorisi { Id = 5, Isim = "İş Mahkemesi" }
            );


        builder.Entity<DosyaDurumu>()
            .HasData
            (
                new DosyaDurumu { Id = 1, Isim = "Açık" },
                new DosyaDurumu { Id = 2, Isim = "Arşiv" },
                new DosyaDurumu { Id = 3, Isim = "Derdest" },
                new DosyaDurumu { Id = 4, Isim = "Hazırlık" },
                new DosyaDurumu { Id = 5, Isim = "İstinaf" },
                new DosyaDurumu { Id = 6, Isim = "Kapalı (Aciz Vesikası)" },
                new DosyaDurumu { Id = 7, Isim = "Kapalı (İnfaz)" },
                new DosyaDurumu { Id = 8, Isim = "Karar" },
                new DosyaDurumu { Id = 9, Isim = "Temyiz" }
            );


        builder.Entity<GorevDurumu>()
            .HasData
            (
                new GorevDurumu { Id = 1, Isim ="Tamamlandı" },
                new GorevDurumu { Id = 2, Isim ="Devam Ediyor" },
                new GorevDurumu { Id = 3, Isim ="İptal" }
            );


        builder.Entity<FinansIslemTuru>()
            .HasData
            (
                new FinansIslemTuru { Id = 1, Isim = "Dosya Geliri" },
                new FinansIslemTuru { Id = 2, Isim = "Dosya Gideri" },
                new FinansIslemTuru { Id = 3, Isim = "Ofis Genel Gideri" },
                new FinansIslemTuru { Id = 4, Isim = "Personel Maaşı" },
                new FinansIslemTuru { Id = 5, Isim = "Personel Avans" },
                new FinansIslemTuru { Id = 6, Isim = "İade" },
                new FinansIslemTuru { Id = 7, Isim = "Kasa Düzeltme" },
                new FinansIslemTuru { Id = 8, Isim = "Transfer" },
                new FinansIslemTuru { Id = 9, Isim = "Serbest Meslek" },
                new FinansIslemTuru { Id = 10, Isim = "Diğer" }
            );


        builder.Entity<DuyuruKategorisi>()
            .HasData
            (
                new DuyuruKategorisi { Id = 1, Isim ="Önemli" },
                new DuyuruKategorisi { Id = 2, Isim ="Normal" },
                new DuyuruKategorisi { Id = 3, Isim ="Uygulama Güncellemesi" }
            );


        builder.Entity<Personel>()
            .HasMany(p => p.SorumluPersonelFinansIslemi)
            .WithOne(s => s.IslemYapan)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Personel>()
            .HasMany(p => p.IlgiliFinansIslemleri)
            .WithOne(i => i.Personel)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<KisiBaglantisi>()
            .HasKey(kb => kb.Id);

        builder.Entity<KisiBaglantisi>()
            .HasOne(kb => kb.Kisi)
            .WithMany(k => k.IlgiliKisiler)
            .HasForeignKey(kb => kb.KisiId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<KisiBaglantisi>()
            .HasOne(kb => kb.IlgiliKisi)
            .WithMany()
            .HasForeignKey(kb => kb.IlgiliKisiId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
