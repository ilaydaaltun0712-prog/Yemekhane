using Microsoft.EntityFrameworkCore;
using Yemekhane.Models;

namespace Yemekhane.Data;

public class YemekhaneContext : DbContext
{
    public YemekhaneContext(DbContextOptions<YemekhaneContext> options) : base(options) { }

    public DbSet<Kullanici> Users { get; set; }
    public DbSet<Gun> Gunler { get; set; }
    public DbSet<Yemek> Yemekler { get; set; }
    public DbSet<GunYemek> GunYemekleri { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gun>().Property(g => g.Ad).HasMaxLength(20);
        modelBuilder.Entity<Yemek>().Property(y => y.Ad).HasMaxLength(100);
        modelBuilder.Entity<Yemek>().HasIndex(y => y.Ad).IsUnique();
        modelBuilder.Entity<GunYemek>().HasIndex(gy => new { gy.GunId, gy.YemekId }).IsUnique();

        modelBuilder.Entity<Gun>().HasData(
            new Gun { Id = 1, Ad = "Pazartesi" },
            new Gun { Id = 2, Ad = "Salı" },
            new Gun { Id = 3, Ad = "Çarşamba" },
            new Gun { Id = 4, Ad = "Perşembe" },
            new Gun { Id = 5, Ad = "Cuma" }
        );
    }
}