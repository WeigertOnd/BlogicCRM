using System.Linq;
using System.Threading.Tasks;
using BlogicCRM.Data;
using BlogicCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogicCRM.Controllers
{
[Authorize]
public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? firstName, string? lastName, string? email, string? phone, string? birthNumber, int? age)
        {
            var clients = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                var s = firstName.Trim();
                clients = clients.Where(c => c.FirstName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                var s = lastName.Trim();
                clients = clients.Where(c => c.LastName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var s = email.Trim();
                clients = clients.Where(c => c.Email.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                var s = phone.Trim();
                clients = clients.Where(c => c.Phone != null && c.Phone.StartsWith(s));
            }

            if (!string.IsNullOrWhiteSpace(birthNumber))
            {
                var s = birthNumber.Trim();
                clients = clients.Where(c => c.BirthNumber.Contains(s));
            }

            if (age.HasValue)
            {
                clients = clients.Where(c => c.Age == age.Value);
            }

            var list = await clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
            ViewData["firstName"] = firstName;
            ViewData["lastName"] = lastName;
            ViewData["email"] = email;
            ViewData["phone"] = phone;
            ViewData["birthNumber"] = birthNumber;
            ViewData["age"] = age?.ToString();
            return View(list);
        }

        public async Task<IActionResult> ExportCsv(string? firstName, string? lastName, string? email, string? phone, string? birthNumber, int? age)
        {
            var clients = _context.Clients.AsQueryable();
            if (!string.IsNullOrWhiteSpace(firstName)) { var s = firstName.Trim(); clients = clients.Where(c => c.FirstName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(lastName)) { var s = lastName.Trim(); clients = clients.Where(c => c.LastName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(email)) { var s = email.Trim(); clients = clients.Where(c => c.Email.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(phone)) { var s = phone.Trim(); clients = clients.Where(c => c.Phone != null && c.Phone.StartsWith(s)); }
            if (!string.IsNullOrWhiteSpace(birthNumber)) { var s = birthNumber.Trim(); clients = clients.Where(c => c.BirthNumber.Contains(s)); }
            if (age.HasValue) { clients = clients.Where(c => c.Age == age.Value); }
            var list = await clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
            var bytes = BlogicCRM.Services.CsvExportService.GenerateClientsCsv(list);
            var ts = DateTime.Now.ToString("yyyyMMdd-HHmm");
            return File(bytes, "text/csv; charset=utf-8", $"klienti-export-{ts}.csv");
        }

        public async Task<IActionResult> ExportSingleCsv(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            var bytes = BlogicCRM.Services.CsvExportService.GenerateClientsCsv(new[] { client });
            var fileName = $"klient-{client.Id}.csv";
            return File(bytes, "text/csv; charset=utf-8", fileName);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.Contracts)
                    .ThenInclude(ct => ct.ManagerAdvisor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null) return NotFound();

            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,BirthNumber,Age")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Klient byl úspěšně vytvořen.";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,BirthNumber,Age")] Client client)
        {
            if (id != client.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id)) return NotFound();
                    else throw;
                }
                TempData["Success"] = "Klient byl úspěšně upraven.";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.Contracts)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null) return NotFound();

            if (client.Contracts != null && client.Contracts.Any())
            {
                ViewBag.ErrorMessage = "Klienta nelze smazat, protože má přiřazené smlouvy.";
                TempData["Error"] = "Klienta nelze smazat, protože má přiřazené smlouvy.";
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Contracts)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            if (client.Contracts != null && client.Contracts.Any())
            {
                ViewBag.ErrorMessage = "Klienta nelze smazat, protože má přiřazené smlouvy.";
                TempData["Error"] = "Klienta nelze smazat, protože má přiřazené smlouvy.";
                return View(client);
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Klient byl úspěšně smazán.";
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
