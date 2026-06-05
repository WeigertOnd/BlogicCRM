using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogicCRM.Data;
using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogicCRM.Controllers
{
[Authorize]
public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? registrationNumber, string? institution, string? clientFirstName, string? clientLastName, string? managerFirstName, string? managerLastName, string? statusFilter)
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ManagerAdvisor)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(registrationNumber))
            {
                var s = registrationNumber.Trim();
                contracts = contracts.Where(c => c.RegistrationNumber.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(institution))
            {
                var s = institution.Trim();
                contracts = contracts.Where(c => c.Institution.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(clientFirstName))
            {
                var s = clientFirstName.Trim();
                contracts = contracts.Where(c => c.Client != null && c.Client.FirstName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(clientLastName))
            {
                var s = clientLastName.Trim();
                contracts = contracts.Where(c => c.Client != null && c.Client.LastName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(managerFirstName))
            {
                var s = managerFirstName.Trim();
                contracts = contracts.Where(c => c.ManagerAdvisor != null && c.ManagerAdvisor.FirstName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(managerLastName))
            {
                var s = managerLastName.Trim();
                contracts = contracts.Where(c => c.ManagerAdvisor != null && c.ManagerAdvisor.LastName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
            {
                if (statusFilter == "Active")
                {
                    contracts = contracts.Where(c => !c.DateEnded.HasValue || c.DateEnded.Value > DateTime.Today);
                }
                else if (statusFilter == "Ended")
                {
                    contracts = contracts.Where(c => c.DateEnded.HasValue && c.DateEnded.Value <= DateTime.Today);
                }
            }

            var list = await contracts.OrderBy(c => c.RegistrationNumber).ToListAsync();
            ViewData["registrationNumber"] = registrationNumber;
            ViewData["institution"] = institution;
            ViewData["clientFirstName"] = clientFirstName;
            ViewData["clientLastName"] = clientLastName;
            ViewData["managerFirstName"] = managerFirstName;
            ViewData["managerLastName"] = managerLastName;
            ViewData["statusFilter"] = statusFilter;
            return View(list);
        }

        public async Task<IActionResult> ExportCsv(string? registrationNumber, string? institution, string? clientFirstName, string? clientLastName, string? managerFirstName, string? managerLastName, string? statusFilter)
        {
            var contracts = _context.Contracts.Include(c => c.Client).Include(c => c.ManagerAdvisor).AsQueryable();
            if (!string.IsNullOrWhiteSpace(registrationNumber)) { var s = registrationNumber.Trim(); contracts = contracts.Where(c => c.RegistrationNumber.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(institution)) { var s = institution.Trim(); contracts = contracts.Where(c => c.Institution.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(clientFirstName)) { var s = clientFirstName.Trim(); contracts = contracts.Where(c => c.Client != null && c.Client.FirstName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(clientLastName)) { var s = clientLastName.Trim(); contracts = contracts.Where(c => c.Client != null && c.Client.LastName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(managerFirstName)) { var s = managerFirstName.Trim(); contracts = contracts.Where(c => c.ManagerAdvisor != null && c.ManagerAdvisor.FirstName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(managerLastName)) { var s = managerLastName.Trim(); contracts = contracts.Where(c => c.ManagerAdvisor != null && c.ManagerAdvisor.LastName.Contains(s)); }
            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
            {
                if (statusFilter == "Active") contracts = contracts.Where(c => !c.DateEnded.HasValue || c.DateEnded.Value > DateTime.Today);
                else if (statusFilter == "Ended") contracts = contracts.Where(c => c.DateEnded.HasValue && c.DateEnded.Value <= DateTime.Today);
            }
            var list = await contracts.OrderBy(c => c.RegistrationNumber).ToListAsync();
            var bytes = BlogicCRM.Services.CsvExportService.GenerateContractsCsv(list);
            var ts = DateTime.Now.ToString("yyyyMMdd-HHmm");
            return File(bytes, "text/csv; charset=utf-8", $"smlouvy-export-{ts}.csv");
        }

        public async Task<IActionResult> ExportSingleCsv(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ManagerAdvisor)
                .Include(c => c.ContractAdvisors).ThenInclude(ca => ca.Advisor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound();

            var bytes = BlogicCRM.Services.CsvExportService.GenerateSingleContractCsv(contract);
            var fileName = $"smlouva-{contract.Id}.csv";
            return File(bytes, "text/csv; charset=utf-8", fileName);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ManagerAdvisor)
                .Include(c => c.ContractAdvisors)
                    .ThenInclude(ca => ca.Advisor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null) return NotFound();

            return View(contract);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ContractFormViewModel
            {
                Clients = await _context.Clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync(),
                Advisors = await _context.Advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync(),
                DateClosed = DateTime.Today,
                DateValidFrom = DateTime.Today
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractFormViewModel vm)
        {


            if (vm.DateEnded.HasValue && vm.DateEnded.Value < vm.DateClosed)
            {
                ModelState.AddModelError(nameof(vm.DateEnded), "Datum ukončení nesmí být dříve než datum uzavření.");
            }

            if (ModelState.IsValid)
            {
                var contract = new Contract
                {
                    RegistrationNumber = vm.RegistrationNumber,
                    Institution = vm.Institution,
                    ClientId = vm.ClientId,
                    ManagerAdvisorId = vm.ManagerAdvisorId,
                    DateClosed = vm.DateClosed,
                    DateValidFrom = vm.DateValidFrom,
                    DateEnded = vm.DateEnded
                };

                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();


                var advisorIds = new HashSet<int>(vm.SelectedAdvisorIds ?? new List<int>());
                advisorIds.Add(vm.ManagerAdvisorId);
                foreach (var advisorId in advisorIds)
                {
                    _context.ContractAdvisors.Add(new ContractAdvisor { ContractId = contract.Id, AdvisorId = advisorId });
                }
                await _context.SaveChangesAsync();

                TempData["Success"] = "Smlouva byla úspěšně vytvořena.";
                return RedirectToAction(nameof(Index));
            }


            vm.Clients = await _context.Clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
            vm.Advisors = await _context.Advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contract = await _context.Contracts
                .Include(c => c.ContractAdvisors)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound();

            var vm = new ContractFormViewModel
            {
                Id = contract.Id,
                RegistrationNumber = contract.RegistrationNumber,
                Institution = contract.Institution,
                ClientId = contract.ClientId,
                ManagerAdvisorId = contract.ManagerAdvisorId,
                DateClosed = contract.DateClosed,
                DateValidFrom = contract.DateValidFrom,
                DateEnded = contract.DateEnded,
                SelectedAdvisorIds = contract.ContractAdvisors?.Select(ca => ca.AdvisorId).ToList() ?? new List<int>(),
                Clients = await _context.Clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync(),
                Advisors = await _context.Advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContractFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (vm.DateEnded.HasValue && vm.DateEnded.Value < vm.DateClosed)
            {
                ModelState.AddModelError(nameof(vm.DateEnded), "Datum ukončení nesmí být dříve než datum uzavření.");
            }

            if (ModelState.IsValid)
            {
                var contract = await _context.Contracts
                    .Include(c => c.ContractAdvisors)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (contract == null) return NotFound();

                contract.RegistrationNumber = vm.RegistrationNumber;
                contract.Institution = vm.Institution;
                contract.ClientId = vm.ClientId;
                contract.ManagerAdvisorId = vm.ManagerAdvisorId;
                contract.DateClosed = vm.DateClosed;
                contract.DateValidFrom = vm.DateValidFrom;
                contract.DateEnded = vm.DateEnded;

                var existing = contract.ContractAdvisors?.ToList() ?? new List<ContractAdvisor>();
                if (existing.Any())
                {
                    _context.ContractAdvisors.RemoveRange(existing);
                    await _context.SaveChangesAsync();
                }

                var advisorIds = new HashSet<int>(vm.SelectedAdvisorIds ?? new List<int>());
                advisorIds.Add(vm.ManagerAdvisorId);
                foreach (var advisorId in advisorIds)
                {
                    _context.ContractAdvisors.Add(new ContractAdvisor { ContractId = contract.Id, AdvisorId = advisorId });
                }

                _context.Contracts.Update(contract);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Smlouva byla úspěšně upravena.";
                return RedirectToAction(nameof(Index));
            }


            vm.Clients = await _context.Clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
            vm.Advisors = await _context.Advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ManagerAdvisor)
                .Include(c => c.ContractAdvisors)
                    .ThenInclude(ca => ca.Advisor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null) return NotFound();

            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.ContractAdvisors)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound();

            if (contract.ContractAdvisors != null && contract.ContractAdvisors.Any())
            {
                _context.ContractAdvisors.RemoveRange(contract.ContractAdvisors);
                await _context.SaveChangesAsync();
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Smlouva byla úspěšně smazána.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndContract(int id, string? returnUrl)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            contract.DateEnded = DateTime.Today;
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Smlouva byla ukončena.";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReactivateContract(int id, string? returnUrl)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            contract.DateEnded = null;
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Smlouva byla obnovena.";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }
    }
}
