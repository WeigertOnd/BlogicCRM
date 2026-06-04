using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogicCRM.Data;
using BlogicCRM.Models;

namespace BlogicCRM.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string? searchString)
        {
            var clients = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var s = searchString.Trim();
                if (int.TryParse(s, out var n))
                {
                    clients = clients.Where(c => c.FirstName.Contains(s) || c.LastName.Contains(s) || c.Email.Contains(s) || c.Phone.Contains(s) || c.BirthNumber.Contains(s) || c.Age == n);
                }
                else
                {
                    clients = clients.Where(c => c.FirstName.Contains(s) || c.LastName.Contains(s) || c.Email.Contains(s) || c.Phone.Contains(s) || c.BirthNumber.Contains(s));
                }
            }

            var list = await clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
            return View(list);
        }

        // GET: Clients/Details/5
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

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
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

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        // POST: Clients/Edit/5
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

        // GET: Clients/Delete/5
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

        // POST: Clients/Delete/5
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
