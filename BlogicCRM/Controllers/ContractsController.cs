using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogicCRM.Data;
using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using System.Collections.Generic;
using System;

namespace BlogicCRM.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(string? searchString)
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ManagerAdvisor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                contracts = contracts.Where(c => c.RegistrationNumber.Contains(searchString) || c.Institution.Contains(searchString) || c.Client.FirstName.Contains(searchString) || c.Client.LastName.Contains(searchString));
            }

            var list = await contracts.OrderBy(c => c.RegistrationNumber).ToListAsync();
            return View(list);
        }

        // GET: Contracts/Details/5
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

        // GET: Contracts/Create
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

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractFormViewModel vm)
        {
            // validate participants
            if (vm.SelectedAdvisorIds == null || !vm.SelectedAdvisorIds.Any())
            {
                ModelState.AddModelError(nameof(vm.SelectedAdvisorIds), "Smlouva musí mít alespoň jednoho účastníka.");
            }

            if (!vm.SelectedAdvisorIds.Contains(vm.ManagerAdvisorId))
            {
                ModelState.AddModelError(nameof(vm.ManagerAdvisorId), "Správce smlouvy musí být zároveň účastníkem smlouvy.");
            }

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

                // add participants
                foreach (var advisorId in vm.SelectedAdvisorIds.Distinct())
                {
                    _context.ContractAdvisors.Add(new ContractAdvisor { ContractId = contract.Id, AdvisorId = advisorId });
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // reload lists
            vm.Clients = await _context.Clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
            vm.Advisors = await _context.Advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
            return View(vm);
        }

        // GET: Contracts/Edit/5
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

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContractFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (vm.SelectedAdvisorIds == null || !vm.SelectedAdvisorIds.Any())
            {
                ModelState.AddModelError(nameof(vm.SelectedAdvisorIds), "Smlouva musí mít alespoň jednoho účastníka.");
            }

            if (!vm.SelectedAdvisorIds.Contains(vm.ManagerAdvisorId))
            {
                ModelState.AddModelError(nameof(vm.ManagerAdvisorId), "Správce smlouvy musí být zároveň účastníkem smlouvy.");
            }

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

                // update M:N ContractAdvisor: remove all existing and add new ones
                var existing = contract.ContractAdvisors?.ToList() ?? new List<ContractAdvisor>();
                if (existing.Any())
                {
                    _context.ContractAdvisors.RemoveRange(existing);
                    await _context.SaveChangesAsync();
                }

                foreach (var advisorId in vm.SelectedAdvisorIds.Distinct())
                {
                    _context.ContractAdvisors.Add(new ContractAdvisor { ContractId = contract.Id, AdvisorId = advisorId });
                }

                _context.Contracts.Update(contract);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // reload lists
            vm.Clients = await _context.Clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
            vm.Advisors = await _context.Advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
            return View(vm);
        }

        // GET: Contracts/Delete/5
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

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.ContractAdvisors)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound();

            // remove M:N links first
            if (contract.ContractAdvisors != null && contract.ContractAdvisors.Any())
            {
                _context.ContractAdvisors.RemoveRange(contract.ContractAdvisors);
                await _context.SaveChangesAsync();
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
