using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogicCRM.Data;
using BlogicCRM.Models;

namespace BlogicCRM.Controllers
{
    public class AdvisorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdvisorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Advisors
        public async Task<IActionResult> Index(string? searchString)
        {
            var advisors = _context.Advisors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                advisors = advisors.Where(a => a.FirstName.Contains(searchString) || a.LastName.Contains(searchString) || a.Email.Contains(searchString));
            }

            var list = await advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
            return View(list);
        }

        // GET: Advisors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var advisor = await _context.Advisors
                .Include(a => a.ManagedContracts)
                    .ThenInclude(c => c.Client)
                .Include(a => a.ContractAdvisors)
                    .ThenInclude(ca => ca.Contract)
                        .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (advisor == null) return NotFound();

            return View(advisor);
        }

        // GET: Advisors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Advisors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,BirthNumber,Age")] Advisor advisor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advisor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Poradce byl úspěšně vytvořen.";
                return RedirectToAction(nameof(Index));
            }
            return View(advisor);
        }

        // GET: Advisors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor == null) return NotFound();
            return View(advisor);
        }

        // POST: Advisors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,BirthNumber,Age")] Advisor advisor)
        {
            if (id != advisor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advisor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvisorExists(advisor.Id)) return NotFound();
                    else throw;
                }
                TempData["Success"] = "Poradce byl úspěšně upraven.";
                return RedirectToAction(nameof(Index));
            }
            return View(advisor);
        }

        // GET: Advisors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var advisor = await _context.Advisors
                .Include(a => a.ManagedContracts)
                .Include(a => a.ContractAdvisors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (advisor == null) return NotFound();

            // Prevent deletion if advisor is assigned to any contract
            if ((advisor.ManagedContracts != null && advisor.ManagedContracts.Any()) || (advisor.ContractAdvisors != null && advisor.ContractAdvisors.Any()))
            {
                ViewBag.ErrorMessage = "Poradce nelze smazat, protože je přiřazen ke smlouvě.";
                TempData["Error"] = "Poradce nelze smazat, protože je přiřazen ke smlouvě.";
            }

            return View(advisor);
        }

        // POST: Advisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advisor = await _context.Advisors
                .Include(a => a.ManagedContracts)
                .Include(a => a.ContractAdvisors)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advisor == null) return NotFound();

            // Do not allow deletion if related contracts exist
            if ((advisor.ManagedContracts != null && advisor.ManagedContracts.Any()) || (advisor.ContractAdvisors != null && advisor.ContractAdvisors.Any()))
            {
                ViewBag.ErrorMessage = "Poradce nelze smazat, protože je přiřazen ke smlouvě.";
                TempData["Error"] = "Poradce nelze smazat, protože je přiřazen ke smlouvě.";
                return View(advisor);
            }

            _context.Advisors.Remove(advisor);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Poradce byl úspěšně smazán.";
            return RedirectToAction(nameof(Index));
        }

        private bool AdvisorExists(int id)
        {
            return _context.Advisors.Any(e => e.Id == id);
        }
    }
}
