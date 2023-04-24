using HukukBuro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Data;

public sealed class VeriTabani : IdentityDbContext<Personel>
{
    public VeriTabani(DbContextOptions<VeriTabani> ayarlar)  : base(ayarlar)
    {
        
    }

    public DbSet<Duyuru> Duyurular { get; set; }
    public DbSet<DuyuruKategorisi> DuyuruKategorileri { get; set; }
}
