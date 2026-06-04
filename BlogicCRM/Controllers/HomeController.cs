using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using BlogicCRM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Linq;

namespace BlogicCRM.Controllers
{
[Authorize]
public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel();

            vm.ClientsCount = await _context.Clients.CountAsync();
            vm.AdvisorsCount = await _context.Advisors.CountAsync();
            vm.ContractsCount = await _context.Contracts.CountAsync();
            vm.ActiveContractsCount = await _context.Contracts.CountAsync(c => !c.DateEnded.HasValue || c.DateEnded.Value > System.DateTime.Today);
            vm.EndedContractsCount = await _context.Contracts.CountAsync(c => c.DateEnded.HasValue && c.DateEnded.Value <= System.DateTime.Today);

            vm.RecentContracts = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ManagerAdvisor)
                .OrderByDescending(c => c.DateClosed)
                .Take(5)
                .ToListAsync();

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
