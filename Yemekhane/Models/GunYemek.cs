namespace Yemekhane.Models;

public class GunYemek
{
    public int Id { get; set; }

    public int GunId { get; set; }
    public Gun Gun { get; set; } = null!;

    public int YemekId { get; set; }
    public Yemek Yemek { get; set; } = null!;
}