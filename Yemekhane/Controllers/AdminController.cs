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
        var menuler = await _context.Menus.OrderBy(m => m.Id).ToListAsync();
        return View(menuler);
    }

    [HttpPost]
    public async Task<IActionResult> Kaydet(List<GunMenu> menuler)
    {
        foreach (var gelen in menuler)
        {
            var kayit = await _context.Menus.FindAsync(gelen.Id);
            if (kayit != null)
            {
                kayit.Icerik = gelen.Icerik ?? "";
            }
        }

        await _context.SaveChangesAsync();
        TempData["Mesaj"] = "Menü kaydedildi.";
        return RedirectToAction("Index");
    }
}