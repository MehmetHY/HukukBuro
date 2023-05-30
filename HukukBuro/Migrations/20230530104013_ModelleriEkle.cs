using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HukukBuro.Migrations
{
    /// <inheritdoc />
    public partial class ModelleriEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyisim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DosyaDurumu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaDurumu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DosyaKategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaKategorileri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DosyaTurleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaTurleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DurusmaAktiviteTurleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurusmaAktiviteTurleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Duyurular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Konu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duyurular", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GorevDurumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GorevDurumlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kisiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TuzelMi = table.Column<bool>(type: "bit", nullable: false),
                    Kisaltma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Soyisim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KimlikNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SirketIsmi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VergiDairesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VergiNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdresBilgisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankaHesapBilgisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EkBilgi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kisiler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ofis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebAdresi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TarafTurleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarafTurleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bildirimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Okundu = table.Column<bool>(type: "bit", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bildirimler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bildirimler_AspNetUsers_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dosyalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuroNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Konu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DosyaTuruId = table.Column<int>(type: "int", nullable: false),
                    DosyaKategorisiId = table.Column<int>(type: "int", nullable: false),
                    DosyaDurumuId = table.Column<int>(type: "int", nullable: false),
                    Mahkeme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcilisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosyalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dosyalar_DosyaDurumu_DosyaDurumuId",
                        column: x => x.DosyaDurumuId,
                        principalTable: "DosyaDurumu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dosyalar_DosyaKategorileri_DosyaKategorisiId",
                        column: x => x.DosyaKategorisiId,
                        principalTable: "DosyaKategorileri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dosyalar_DosyaTurleri_DosyaTuruId",
                        column: x => x.DosyaTuruId,
                        principalTable: "DosyaTurleri",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KisiBaglantilari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisiId = table.Column<int>(type: "int", nullable: false),
                    IlgiliKisiId = table.Column<int>(type: "int", nullable: false),
                    Pozisyon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KisiBaglantilari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KisiBaglantilari_Kisiler_IlgiliKisiId",
                        column: x => x.IlgiliKisiId,
                        principalTable: "Kisiler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KisiBaglantilari_Kisiler_KisiId",
                        column: x => x.KisiId,
                        principalTable: "Kisiler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KisiBelgeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisiId = table.Column<int>(type: "int", nullable: false),
                    OzelMi = table.Column<bool>(type: "bit", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uzanti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Boyut = table.Column<long>(type: "bigint", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KisiBelgeleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KisiBelgeleri_Kisiler_KisiId",
                        column: x => x.KisiId,
                        principalTable: "Kisiler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisiId = table.Column<int>(type: "int", nullable: false),
                    Konu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TamamlandiMi = table.Column<bool>(type: "bit", nullable: false),
                    SorumluId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Randevular_AspNetUsers_SorumluId",
                        column: x => x.SorumluId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Randevular_Kisiler_KisiId",
                        column: x => x.KisiId,
                        principalTable: "Kisiler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BolgeAdliyeMahkemesiBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    KararNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KararTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TebligTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KararOzeti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mahkeme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GondermeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsasNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BolgeAdliyeMahkemesiBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BolgeAdliyeMahkemesiBilgileri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DosyaBaglantilari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    IlgiliDosyaId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaBaglantilari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosyaBaglantilari_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DosyaBaglantilari_Dosyalar_IlgiliDosyaId",
                        column: x => x.IlgiliDosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DosyaBelgeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uzanti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Boyut = table.Column<long>(type: "bigint", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaBelgeleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosyaBelgeleri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DosyaPersonel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    PersonelId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaPersonel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosyaPersonel_AspNetUsers_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DosyaPersonel_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Durusmalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    AktiviteTuruId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tamamlandi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Durusmalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Durusmalar_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Durusmalar_DurusmaAktiviteTurleri_AktiviteTuruId",
                        column: x => x.AktiviteTuruId,
                        principalTable: "DurusmaAktiviteTurleri",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinansIslemleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Miktar = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SonOdemeTarhi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Odendi = table.Column<bool>(type: "bit", nullable: false),
                    OdemeTarhi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IslemTuru = table.Column<int>(type: "int", nullable: false),
                    IslemYapanId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MakbuzKesildiMi = table.Column<bool>(type: "bit", nullable: false),
                    MakbuzTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MakbuzNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KisiBaglantisiVar = table.Column<bool>(type: "bit", nullable: false),
                    KisiId = table.Column<int>(type: "int", nullable: true),
                    DosyaBaglantisiVar = table.Column<bool>(type: "bit", nullable: false),
                    DosyaId = table.Column<int>(type: "int", nullable: true),
                    PersonelBaglantisiVar = table.Column<bool>(type: "bit", nullable: false),
                    PersonelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinansIslemleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinansIslemleri_AspNetUsers_IslemYapanId",
                        column: x => x.IslemYapanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinansIslemleri_AspNetUsers_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinansIslemleri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinansIslemleri_Kisiler_KisiId",
                        column: x => x.KisiId,
                        principalTable: "Kisiler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Gorevler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaglantiTuru = table.Column<int>(type: "int", nullable: false),
                    KisiId = table.Column<int>(type: "int", nullable: true),
                    DosyaId = table.Column<int>(type: "int", nullable: true),
                    Konu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SorumluId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurumId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gorevler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gorevler_AspNetUsers_SorumluId",
                        column: x => x.SorumluId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gorevler_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gorevler_GorevDurumlari_DurumId",
                        column: x => x.DurumId,
                        principalTable: "GorevDurumlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gorevler_Kisiler_KisiId",
                        column: x => x.KisiId,
                        principalTable: "Kisiler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KararBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    KararNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KararTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TebligTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KararOzeti = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KararBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KararBilgileri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KararDuzeltmeBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    KararNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KararTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TebligTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KararOzeti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mahkeme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GondermeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsasNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KararDuzeltmeBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KararDuzeltmeBilgileri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KesinlesmeBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    KesinlesmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KararOzeti = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KesinlesmeBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KesinlesmeBilgileri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TarafKisiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisiId = table.Column<int>(type: "int", nullable: false),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    KarsiTaraf = table.Column<bool>(type: "bit", nullable: false),
                    TarafTuruId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarafKisiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarafKisiler_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TarafKisiler_Kisiler_KisiId",
                        column: x => x.KisiId,
                        principalTable: "Kisiler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TarafKisiler_TarafTurleri_TarafTuruId",
                        column: x => x.TarafTuruId,
                        principalTable: "TarafTurleri",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TemyizBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    KararNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KararTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TebligTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KararOzeti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mahkeme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GondermeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsasNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemyizBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemyizBilgileri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "DosyaYoneticisi", null, "DosyaYoneticisi", "DOSYAYONETICISI" },
                    { "DuyuruYoneticisi", null, "DuyuruYoneticisi", "DUYURUYONETICISI" },
                    { "FinansYoneticisi", null, "FinansYoneticisi", "FINANSYONETICISI" },
                    { "GorevYoneticisi", null, "GorevYoneticisi", "GOREVYONETICISI" },
                    { "KisiYoneticisi", null, "KisiYoneticisi", "KISIYONETICISI" },
                    { "PersonelYoneticisi", null, "PersonelYoneticisi", "PERSONELYONETICISI" },
                    { "RandevuYoneticisi", null, "RandevuYoneticisi", "RANDEVUYONETICISI" },
                    { "RolYoneticisi", null, "RolYoneticisi", "ROLYONETICISI" }
                });

            migrationBuilder.InsertData(
                table: "DosyaDurumu",
                columns: new[] { "Id", "Isim" },
                values: new object[,]
                {
                    { 1, "Açık" },
                    { 2, "Arşiv" },
                    { 3, "Derdest" },
                    { 4, "Hazırlık" },
                    { 5, "İstinaf" },
                    { 6, "Kapalı (Aciz Vesikası)" },
                    { 7, "Kapalı (İnfaz)" },
                    { 8, "Karar" },
                    { 9, "Temyiz" }
                });

            migrationBuilder.InsertData(
                table: "DosyaKategorileri",
                columns: new[] { "Id", "Isim" },
                values: new object[,]
                {
                    { 1, "Asliye Hukuk" },
                    { 2, "Aylık Hukuk Danışmanlık" },
                    { 3, "İcra" },
                    { 4, "İdare Mahkemesi" },
                    { 5, "İş Mahkemesi" }
                });

            migrationBuilder.InsertData(
                table: "DosyaTurleri",
                columns: new[] { "Id", "Isim" },
                values: new object[,]
                {
                    { 1, "Dava" },
                    { 2, "Danışmanlık" },
                    { 3, "İcra" },
                    { 4, "Arabuluculuk" },
                    { 5, "Soruşturma" }
                });

            migrationBuilder.InsertData(
                table: "DurusmaAktiviteTurleri",
                columns: new[] { "Id", "Isim" },
                values: new object[,]
                {
                    { 1, "Duruşma" },
                    { 2, "Keşif" },
                    { 3, "İnceleme" }
                });

            migrationBuilder.InsertData(
                table: "GorevDurumlari",
                columns: new[] { "Id", "Isim" },
                values: new object[,]
                {
                    { 1, "Tamamlandı" },
                    { 2, "Devam Ediyor" },
                    { 3, "İptal" }
                });

            migrationBuilder.InsertData(
                table: "TarafTurleri",
                columns: new[] { "Id", "Isim" },
                values: new object[,]
                {
                    { 1, "Davacı" },
                    { 2, "Davalı" },
                    { 3, "Diğer" },
                    { 4, "Avukat" },
                    { 5, "Alacaklı" },
                    { 6, "Borçlu" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bildirimler_PersonelId",
                table: "Bildirimler",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_BolgeAdliyeMahkemesiBilgileri_DosyaId",
                table: "BolgeAdliyeMahkemesiBilgileri",
                column: "DosyaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DosyaBaglantilari_DosyaId",
                table: "DosyaBaglantilari",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaBaglantilari_IlgiliDosyaId",
                table: "DosyaBaglantilari",
                column: "IlgiliDosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaBelgeleri_DosyaId",
                table: "DosyaBelgeleri",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dosyalar_DosyaDurumuId",
                table: "Dosyalar",
                column: "DosyaDurumuId");

            migrationBuilder.CreateIndex(
                name: "IX_Dosyalar_DosyaKategorisiId",
                table: "Dosyalar",
                column: "DosyaKategorisiId");

            migrationBuilder.CreateIndex(
                name: "IX_Dosyalar_DosyaTuruId",
                table: "Dosyalar",
                column: "DosyaTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaPersonel_DosyaId",
                table: "DosyaPersonel",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaPersonel_PersonelId",
                table: "DosyaPersonel",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Durusmalar_AktiviteTuruId",
                table: "Durusmalar",
                column: "AktiviteTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Durusmalar_DosyaId",
                table: "Durusmalar",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_FinansIslemleri_DosyaId",
                table: "FinansIslemleri",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_FinansIslemleri_IslemYapanId",
                table: "FinansIslemleri",
                column: "IslemYapanId");

            migrationBuilder.CreateIndex(
                name: "IX_FinansIslemleri_KisiId",
                table: "FinansIslemleri",
                column: "KisiId");

            migrationBuilder.CreateIndex(
                name: "IX_FinansIslemleri_PersonelId",
                table: "FinansIslemleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_DosyaId",
                table: "Gorevler",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_DurumId",
                table: "Gorevler",
                column: "DurumId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_KisiId",
                table: "Gorevler",
                column: "KisiId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_SorumluId",
                table: "Gorevler",
                column: "SorumluId");

            migrationBuilder.CreateIndex(
                name: "IX_KararBilgileri_DosyaId",
                table: "KararBilgileri",
                column: "DosyaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KararDuzeltmeBilgileri_DosyaId",
                table: "KararDuzeltmeBilgileri",
                column: "DosyaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KesinlesmeBilgileri_DosyaId",
                table: "KesinlesmeBilgileri",
                column: "DosyaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KisiBaglantilari_IlgiliKisiId",
                table: "KisiBaglantilari",
                column: "IlgiliKisiId");

            migrationBuilder.CreateIndex(
                name: "IX_KisiBaglantilari_KisiId",
                table: "KisiBaglantilari",
                column: "KisiId");

            migrationBuilder.CreateIndex(
                name: "IX_KisiBelgeleri_KisiId",
                table: "KisiBelgeleri",
                column: "KisiId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_KisiId",
                table: "Randevular",
                column: "KisiId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_SorumluId",
                table: "Randevular",
                column: "SorumluId");

            migrationBuilder.CreateIndex(
                name: "IX_TarafKisiler_DosyaId",
                table: "TarafKisiler",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_TarafKisiler_KisiId",
                table: "TarafKisiler",
                column: "KisiId");

            migrationBuilder.CreateIndex(
                name: "IX_TarafKisiler_TarafTuruId",
                table: "TarafKisiler",
                column: "TarafTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_TemyizBilgileri_DosyaId",
                table: "TemyizBilgileri",
                column: "DosyaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Bildirimler");

            migrationBuilder.DropTable(
                name: "BolgeAdliyeMahkemesiBilgileri");

            migrationBuilder.DropTable(
                name: "DosyaBaglantilari");

            migrationBuilder.DropTable(
                name: "DosyaBelgeleri");

            migrationBuilder.DropTable(
                name: "DosyaPersonel");

            migrationBuilder.DropTable(
                name: "Durusmalar");

            migrationBuilder.DropTable(
                name: "Duyurular");

            migrationBuilder.DropTable(
                name: "FinansIslemleri");

            migrationBuilder.DropTable(
                name: "Gorevler");

            migrationBuilder.DropTable(
                name: "KararBilgileri");

            migrationBuilder.DropTable(
                name: "KararDuzeltmeBilgileri");

            migrationBuilder.DropTable(
                name: "KesinlesmeBilgileri");

            migrationBuilder.DropTable(
                name: "KisiBaglantilari");

            migrationBuilder.DropTable(
                name: "KisiBelgeleri");

            migrationBuilder.DropTable(
                name: "Ofis");

            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "TarafKisiler");

            migrationBuilder.DropTable(
                name: "TemyizBilgileri");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DurusmaAktiviteTurleri");

            migrationBuilder.DropTable(
                name: "GorevDurumlari");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Kisiler");

            migrationBuilder.DropTable(
                name: "TarafTurleri");

            migrationBuilder.DropTable(
                name: "Dosyalar");

            migrationBuilder.DropTable(
                name: "DosyaDurumu");

            migrationBuilder.DropTable(
                name: "DosyaKategorileri");

            migrationBuilder.DropTable(
                name: "DosyaTurleri");
        }
    }
}
