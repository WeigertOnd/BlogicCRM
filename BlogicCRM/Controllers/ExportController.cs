using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogicCRM.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Linq;
using BlogicCRM.Services;
using System;

namespace BlogicCRM.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Export
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Export(bool clients, bool advisors, bool contracts)
        {
            if (!clients && !advisors && !contracts)
            {
                ModelState.AddModelError(string.Empty, "Vyberte alespoň jednu část k exportu.");
                return View("Index");
            }

            var files = new System.Collections.Generic.List<(string Name, byte[] Content)>();

            if (clients)
            {
                var list = await _context.Clients.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();
                files.Add(("klienti-export.csv", CsvExportService.GenerateClientsCsv(list)));
            }

            if (advisors)
            {
                var list = await _context.Advisors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();
                files.Add(("poradci-export.csv", CsvExportService.GenerateAdvisorsCsv(list)));
            }

            if (contracts)
            {
                var list = await _context.Contracts.Include(c => c.Client).Include(c => c.ManagerAdvisor).OrderBy(c => c.RegistrationNumber).ToListAsync();
                files.Add(("smlouvy-export.csv", CsvExportService.GenerateContractsCsv(list)));
            }

            if (files.Count == 1)
            {
                var f = files[0];
                var ts = DateTime.Now.ToString("yyyyMMdd-HHmm");
                var name = f.Name;
                // attach timestamp before extension
                var outName = System.IO.Path.GetFileNameWithoutExtension(name) + "-" + ts + System.IO.Path.GetExtension(name);
                return File(f.Content, "text/csv; charset=utf-8", outName);
            }

            // Multiple files -> create zip
            using var ms = new MemoryStream();
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var f in files)
                {
                    var entry = archive.CreateEntry(f.Name);
                    using var es = entry.Open();
                    es.Write(f.Content, 0, f.Content.Length);
                }
            }
            // No-op patch: ensure zip filename is export-dat-yyyyMMdd-HHmm.zip and entries use provided byte[].
            ms.Position = 0;
            var zipTs = DateTime.Now.ToString("yyyyMMdd-HHmm");
            return File(ms.ToArray(), "application/zip", $"export-dat-{zipTs}.zip");
        }
    }
}
