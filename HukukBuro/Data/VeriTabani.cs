using HukukBuro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Data;

public sealed class VeriTabani : IdentityDbContext<Personel>
{
    public DbSet<Bildirim> Bildirimler { get; set; }
    public DbSet<Duyuru> Duyurular { get; set; }
    public DbSet<BolgeAdliyeMahkemesiBilgileri> BolgeAdliyeMahkemesiBilgileri { get; set; }
    public DbSet<Dosya> Dosyalar { get; set; }
    public DbSet<DosyaBaglantisi> DosyaBaglantilari { get; set; }
    public DbSet<DosyaBelgesi> DosyaBelgeleri { get; set; }
    public DbSet<DosyaDurumu> DosyaDurumu { get; set; }
    public DbSet<DosyaPersonel> DosyaPersonel { get; set; }
    public DbSet<DosyaKategorisi> DosyaKategorileri { get; set; }
    public DbSet<DosyaTuru> DosyaTurleri { get; set; }
    public DbSet<Durusma> Durusmalar { get; set; }
    public DbSet<DurusmaAktiviteTuru> DurusmaAktiviteTurleri { get; set; }
    public DbSet<FinansIslemi> FinansIslemleri { get; set; }
    public DbSet<Gorev> Gorevler { get; set; }
    public DbSet<GorevDurumu> GorevDurumlari { get; set; }
    public DbSet<KararBilgileri> KararBilgileri { get; set; }
    public DbSet<KararDuzeltmeBilgileri> KararDuzeltmeBilgileri { get; set; }
    public DbSet<KesinlesmeBilgileri> KesinlesmeBilgileri { get; set; }
    public DbSet<Kisi> Kisiler { get; set; }
    public DbSet<KisiBaglantisi> KisiBaglantilari { get; set; }
    public DbSet<KisiBelgesi> KisiBelgeleri { get; set; }
    public DbSet<Ofis> Ofis { get; set; }
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

        builder.Entity<Dosya>()
            .HasOne(d => d.DosyaTuru)
            .WithMany()
            .HasForeignKey(d => d.DosyaTuruId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Dosya>()
            .HasOne(d => d.DosyaDurumu)
            .WithMany()
            .HasForeignKey(d => d.DosyaDurumuId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Dosya>()
            .HasOne(d => d.DosyaKategorisi)
            .WithMany()
            .HasForeignKey(d => d.DosyaKategorisiId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<DosyaBaglantisi>()
            .HasOne(d => d.IlgiliDosya)
            .WithMany()
            .HasForeignKey(d => d.IlgiliDosyaId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Durusma>()
            .HasOne(d => d.AktiviteTuru)
            .WithMany()
            .HasForeignKey(d => d.AktiviteTuruId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<TarafKisi>()
            .HasOne(t => t.TarafTuru)
            .WithMany()
            .HasForeignKey(t => t.TarafTuruId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Gorev>()
            .HasOne(g => g.Durum)
            .WithMany()
            .HasForeignKey(g => g.DurumId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Gorev>()
            .HasOne(g => g.Kisi)
            .WithMany(k => k.IlgiliGorevler)
            .HasForeignKey(g => g.KisiId);

        builder.Entity<Gorev>()
            .HasOne(g => g.Dosya)
            .WithMany(d => d.IlgiliGorevler)
            .HasForeignKey(g => g.DosyaId);

        builder.Entity<Gorev>()
            .HasOne(g => g.Sorumlu)
            .WithMany(k => k.SorumluGorevler)
            .HasForeignKey(g => g.SorumluId);


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
                new DosyaTuru { Id = Sabit.DosyaTuru.DavaId, Isim = Sabit.DosyaTuru.Dava },
                new DosyaTuru { Id = Sabit.DosyaTuru.DanismanlikId, Isim = Sabit.DosyaTuru.Danismanlik },
                new DosyaTuru { Id = Sabit.DosyaTuru.IcraId, Isim = Sabit.DosyaTuru.Icra },
                new DosyaTuru { Id = Sabit.DosyaTuru.ArabuluculukId, Isim = Sabit.DosyaTuru.Arabuluculuk },
                new DosyaTuru { Id = Sabit.DosyaTuru.SorusturmaId, Isim = Sabit.DosyaTuru.Sorusturma }
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
                new GorevDurumu { Id = 1, Isim = "Tamamlandı" },
                new GorevDurumu { Id = 2, Isim = "Devam Ediyor" },
                new GorevDurumu { Id = 3, Isim = "İptal" }
            );


        builder.Entity<FinansIslemi>()
            .HasOne(f => f.IslemYapan)
            .WithMany(p => p.SorumluFinansIslemleri)
            .HasForeignKey(f => f.IslemYapanId);

        builder.Entity<FinansIslemi>()
            .HasOne(f => f.Personel)
            .WithMany(p => p.IlgiliFinansIslemleri)
            .HasForeignKey(f => f.PersonelId);

        builder.Entity<FinansIslemi>()
            .HasOne(f => f.Kisi)
            .WithMany(k => k.IlgiliFinansIslemleri)
            .HasForeignKey(f => f.KisiId);

        builder.Entity<FinansIslemi>()
            .HasOne(f => f.Dosya)
            .WithMany(d => d.IlgiliFinansIslemleri)
            .HasForeignKey(f => f.DosyaId);


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


        builder.Entity<TarafKisi>()
            .HasKey(t => t.Id);

        builder.Entity<TarafKisi>()
            .HasOne(t => t.Dosya)
            .WithMany(d => d.Taraflar)
            .HasForeignKey(t => t.DosyaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TarafKisi>()
            .HasOne(t => t.Kisi)
            .WithMany(k => k.IlgiliDosyalar)
            .HasForeignKey(t => t.KisiId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<DosyaPersonel>()
            .HasKey(dp => dp.Id);

        builder.Entity<DosyaPersonel>()
            .HasOne(dp => dp.Dosya)
            .WithMany(d => d.SorumluPersonel)
            .HasForeignKey(dp => dp.DosyaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DosyaPersonel>()
            .HasOne(dp => dp.Personel)
            .WithMany(p => p.SorumluDosyalar)
            .HasForeignKey(dp => dp.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole
                {
                    Id = Sabit.Yetki.Kisi,
                    Name = Sabit.Yetki.Kisi,
                    NormalizedName = Sabit.Yetki.Kisi.ToUpper()
                },
                new IdentityRole
                {
                    Id = Sabit.Yetki.Dosya,
                    Name = Sabit.Yetki.Dosya,
                    NormalizedName = Sabit.Yetki.Dosya.ToUpper()
                },
                new IdentityRole
                {
                    Id = Sabit.Yetki.Personel,
                    Name = Sabit.Yetki.Personel,
                    NormalizedName = Sabit.Yetki.Personel.ToUpper()
                },
                new IdentityRole
                {
                    Id = Sabit.Yetki.Gorev,
                    Name = Sabit.Yetki.Gorev,
                    NormalizedName = Sabit.Yetki.Gorev.ToUpper()
                },
                new IdentityRole
                {
                    Id = Sabit.Yetki.Duyuru,
                    Name = Sabit.Yetki.Duyuru,
                    NormalizedName = Sabit.Yetki.Duyuru.ToUpper()
                },
                new IdentityRole
                {
                    Id = Sabit.Yetki.Rol,
                    Name = Sabit.Yetki.Rol,
                    NormalizedName = Sabit.Yetki.Rol.ToUpper()
                },
                new IdentityRole
                {
                    Id = Sabit.Yetki.Finans,
                    Name = Sabit.Yetki.Finans,
                    NormalizedName = Sabit.Yetki.Finans.ToUpper()
                },
                new IdentityRole
                {
                    Id = Sabit.Yetki.Randevu,
                    Name = Sabit.Yetki.Randevu,
                    NormalizedName = Sabit.Yetki.Randevu.ToUpper()
                }
            );


        builder.Entity<DosyaBaglantisi>()
            .HasKey(db => db.Id);

        builder.Entity<DosyaBaglantisi>()
            .HasOne(db => db.Dosya)
            .WithMany(d => d.IlgiliDosyalar)
            .HasForeignKey(db => db.DosyaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DosyaBaglantisi>()
            .HasOne(db => db.IlgiliDosya)
            .WithMany()
            .HasForeignKey(db => db.IlgiliDosyaId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Bildirim>()
            .HasOne(b => b.Personel)
            .WithMany(p => p.Bildirimler)
            .HasForeignKey(b => b.PersonelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
