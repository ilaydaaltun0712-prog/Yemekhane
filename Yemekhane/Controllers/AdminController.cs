using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yemekhane.Data;
using Yemekhane.Models;

namespace Yemekhane.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly YemekhaneContext _context;

    public AdminController(YemekhaneContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var gunler = await _context.Gunler
            .Include(g => g.GunYemekleri)
                .ThenInclude(gy => gy.Yemek)
            .OrderBy(g => g.Id)
            .ToListAsync();

        var model = gunler.Select(g => new GunFormu
        {
            GunId = g.Id,
            GunAdi = g.Ad,
            Yemekler = string.Join("\n", g.GunYemekleri
                .OrderBy(gy => gy.Id)
                .Select(gy => gy.Yemek.Ad))
        }).ToList();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Kaydet(List<GunFormu> menuler)
    {
        var karsilastirici = StringComparer.Create(new CultureInfo("tr-TR"), true);

        foreach (var form in menuler)
        {
            var gun = await _context.Gunler
                .Include(g => g.GunYemekleri)
                .FirstOrDefaultAsync(g => g.Id == form.GunId);

            if (gun == null) continue;

            // 1. Günün eski bağlantılarını sil
            _context.GunYemekleri.RemoveRange(gun.GunYemekleri);
            await _context.SaveChangesAsync();

            // 2. Kutudaki satırları yemek adlarına ayır
            var adlar = (form.Yemekler ?? "")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(a => a.Length > 100 ? a[..100] : a)
                .Distinct(karsilastirici);

            // 3. Her yemeği bul (yoksa ekle) ve güne bağla
            foreach (var ad in adlar)
            {
                var yemek = await _context.Yemekler.FirstOrDefaultAsync(y => y.Ad == ad);
                if (yemek == null)
                {
                    yemek = new Yemek { Ad = ad };
                    _context.Yemekler.Add(yemek);
                }

                _context.GunYemekleri.Add(new GunYemek { Gun = gun, Yemek = yemek });
            }

            await _context.SaveChangesAsync();
        }

        TempData["Mesaj"] = "Menü kaydedildi.";
        return RedirectToAction("Index");
    }
}