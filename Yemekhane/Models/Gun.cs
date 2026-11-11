namespace Yemekhane.Models;

public class Gun
{
    public int Id { get; set; }
    public string Ad { get; set; } = "";
    public List<GunYemek> GunYemekleri { get; set; } = new();
}