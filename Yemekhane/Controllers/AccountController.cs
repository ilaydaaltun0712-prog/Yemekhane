using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yemekhane.Data;
using Yemekhane.Models;

namespace Yemekhane.Controllers;

public class AccountController : Controller
{
    private readonly YemekhaneContext _context;

    public AccountController(YemekhaneContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string kullaniciAdi, string sifre)
    {
        var kullanici = await _context.Users
            .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi);

        if (kullanici != null)
        {
            var hasher = new PasswordHasher<Kullanici>();
            var sonuc = hasher.VerifyHashedPassword(kullanici, kullanici.SifreHash, sifre ?? "");

            if (sonuc != PasswordVerificationResult.Failed)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, kullanici.KullaniciAdi) };
                var kimlik = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(kimlik));

                return RedirectToAction("Index", "Admin");
            }
        }

        ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}