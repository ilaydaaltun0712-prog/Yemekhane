using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yemekhane.Data;

namespace Yemekhane.Controllers;

public class MenuController : Controller
{
    private readonly YemekhaneContext _context;

    public MenuController(YemekhaneContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Liste()
    {
        var gunler = await _context.Gunler
            .Include(g => g.GunYemekleri)
            .ThenInclude(gy => gy.Yemek)
            .OrderBy(g => g.Id)
            .ToListAsync();

        return View(gunler);
    }
}