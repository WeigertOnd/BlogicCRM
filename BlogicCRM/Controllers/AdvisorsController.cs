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
public class AdvisorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdvisorsController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string? firstName, string? lastName, string? email, string? phone, string? birthNumber, int? age)
        {
            var advisors = _context.Advisors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                var s = firstName.Trim();
                advisors = advisors.Where(a => a.FirstName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                var s = lastName.Trim();
                advisors = advisors.Where(a => a.LastName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var s = email.Trim();
                advisors = advisors.Where(a => a.Email.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                var s = phone.Trim();
                advisors = advisors.Where(a => a.Phone != null && a.Phone.StartsWith(s));
            }

            if (!string.IsNullOrWhiteSpace(birthNumber))
            {
                var s = birthNumber.Trim();
                advisors = advisors.Where(a => a.BirthNumber.Contains(s));
            }

            if (age.HasValue)
            {
                advisors = advisors.Where(a => a.Age == age.Value);
            }

            var list = await advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
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
            var advisors = _context.Advisors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(firstName)) { var s = firstName.Trim(); advisors = advisors.Where(a => a.FirstName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(lastName)) { var s = lastName.Trim(); advisors = advisors.Where(a => a.LastName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(email)) { var s = email.Trim(); advisors = advisors.Where(a => a.Email.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(phone)) { var s = phone.Trim(); advisors = advisors.Where(a => a.Phone != null && a.Phone.StartsWith(s)); }
            if (!string.IsNullOrWhiteSpace(birthNumber)) { var s = birthNumber.Trim(); advisors = advisors.Where(a => a.BirthNumber.Contains(s)); }
            if (age.HasValue) { advisors = advisors.Where(a => a.Age == age.Value); }
            var list = await advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
            var bytes = BlogicCRM.Services.CsvExportService.GenerateAdvisorsCsv(list);
            var ts = DateTime.Now.ToString("yyyyMMdd-HHmm");
            return File(bytes, "text/csv; charset=utf-8", $"poradci-export-{ts}.csv");
        }


        public async Task<IActionResult> ExportSingleCsv(int id)
        {
            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor == null) return NotFound();
            var bytes = BlogicCRM.Services.CsvExportService.GenerateAdvisorsCsv(new[] { advisor });
            var fileName = $"poradce-{advisor.Id}.csv";
            return File(bytes, "text/csv; charset=utf-8", fileName);
        }


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


        public IActionResult Create()
        {
            return View();
        }


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


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor == null) return NotFound();
            return View(advisor);
        }


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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var advisor = await _context.Advisors
                .Include(a => a.ManagedContracts)
                .Include(a => a.ContractAdvisors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (advisor == null) return NotFound();

            // Zabránit smazání, pokud je poradce přiřazen ke smlouvě
            if ((advisor.ManagedContracts != null && advisor.ManagedContracts.Any()) || (advisor.ContractAdvisors != null && advisor.ContractAdvisors.Any()))
            {
                ViewBag.ErrorMessage = "Poradce nelze smazat, protože je přiřazen ke smlouvě.";
                TempData["Error"] = "Poradce nelze smazat, protože je přiřazen ke smlouvě.";
            }

            return View(advisor);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advisor = await _context.Advisors
                .Include(a => a.ManagedContracts)
                .Include(a => a.ContractAdvisors)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advisor == null) return NotFound();

            // Nepovolovat smazání, pokud existují související smlouvy
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
