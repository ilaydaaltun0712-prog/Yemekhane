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
        var menuler = await _context.Menus.OrderBy(m => m.Id).ToListAsync();
        return View(menuler);
    }
}