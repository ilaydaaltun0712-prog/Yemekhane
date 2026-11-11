using Microsoft.EntityFrameworkCore;
using Yemekhane.Models;

namespace Yemekhane.Data;

public class YemekhaneContext : DbContext
{
    public YemekhaneContext(DbContextOptions<YemekhaneContext> options) : base(options) { }

    public DbSet<Kullanici> Users { get; set; }
    public DbSet<GunMenu> Menus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GunMenu>().HasData(
            new GunMenu { Id = 1, Gun = "Pazartesi", Icerik = "" },
            new GunMenu { Id = 2, Gun = "Salı", Icerik = "" },
            new GunMenu { Id = 3, Gun = "Çarşamba", Icerik = "" },
            new GunMenu { Id = 4, Gun = "Perşembe", Icerik = "" },
            new GunMenu { Id = 5, Gun = "Cuma", Icerik = "" }
        );
    }
}