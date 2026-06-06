using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BlogicCRM.Data;
using BlogicCRM.ViewModels;
using BlogicCRM.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BlogicCRM.Controllers
{
    public class AccountController : Controller
    {
        private const string DemoEmail = "admin@blogiccrm.cz";
        private const string DemoPassword = "Admin123";
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            var email = vm.Email.Trim().ToLowerInvariant();

            if (email == DemoEmail && vm.Password == DemoPassword)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, DemoEmail), new Claim(ClaimTypes.Email, DemoEmail) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }


            var user = _context.UserAccounts.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                if (VerifyPassword(vm.Password, user.PasswordSalt, user.PasswordHash))
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Email), new Claim(ClaimTypes.Email, user.Email) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Neplatný e-mail nebo heslo.");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var email = vm.Email.Trim().ToLowerInvariant();


            if (_context.UserAccounts.Any(u => u.Email == email))
            {
                ModelState.AddModelError(string.Empty, "Uživatel s tímto e-mailem již existuje.");
                return View(vm);
            }

            var salt = GenerateSalt();
            var hash = HashPassword(vm.Password, salt);

            var user = new UserAccount
            {
                Email = email,
                PasswordSalt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(hash),
                CreatedAt = DateTime.UtcNow
            };

            _context.UserAccounts.Add(user);
            try
            {
                _context.SaveChanges();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Uživatel s tímto e-mailem již existuje.");
                return View(vm);
            }

            TempData["Success"] = "Registrace proběhla úspěšně. Nyní se můžete přihlásit.";
            return RedirectToAction("Login");
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(32);
        }

        private static bool VerifyPassword(string password, string saltBase64, string hashBase64)
        {
            var salt = Convert.FromBase64String(saltBase64);
            var expected = Convert.FromBase64String(hashBase64);
            var actual = HashPassword(password, salt);
            return actual.SequenceEqual(expected);
        }
    }
}
