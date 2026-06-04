using BlogicCRM.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogicCRM.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Advisor> Advisors => Set<Advisor>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<ContractAdvisor> ContractAdvisors => Set<ContractAdvisor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContractAdvisor>()
                .HasKey(ca => new { ca.ContractId, ca.AdvisorId });

            modelBuilder.Entity<ContractAdvisor>()
                .HasOne(ca => ca.Contract)
                .WithMany(c => c.ContractAdvisors)
                .HasForeignKey(ca => ca.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractAdvisor>()
                .HasOne(ca => ca.Advisor)
                .WithMany(a => a.ContractAdvisors)
                .HasForeignKey(ca => ca.AdvisorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Contract -> ManagerAdvisor (one Advisor manages many Contracts)
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.ManagerAdvisor)
                .WithMany(a => a.ManagedContracts)
                .HasForeignKey(c => c.ManagerAdvisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Client -> Contracts: prevent cascade delete to avoid accidental contract deletions
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Contracts)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data (minimal)
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, FirstName = "Jan", LastName = "Novák", Email = "jan.novak@priklad.cz", Phone = "+420123456789", BirthNumber = "800101/1234", Age = 44 },
                new Client { Id = 2, FirstName = "Petra", LastName = "Svobodová", Email = "petra.svobodova@priklad.cz", Phone = "+420987654321", BirthNumber = "900202/4321", Age = 34 }
            );

            modelBuilder.Entity<Advisor>().HasData(
                new Advisor { Id = 1, FirstName = "Anna", LastName = "Králová", Email = "anna.kralova@firma.cz", Phone = "+420111222333", BirthNumber = "750303/1111", Age = 49 },
                new Advisor { Id = 2, FirstName = "Martin", LastName = "Dvořák", Email = "martin.dvorak@firma.cz", Phone = "+420444555666", BirthNumber = "850404/2222", Age = 39 },
                new Advisor { Id = 3, FirstName = "Lucie", LastName = "Novotná", Email = "lucie.novotna@firma.cz", Phone = "+420777888999", BirthNumber = "820505/3333", Age = 41 }
            );

            modelBuilder.Entity<Contract>().HasData(
                new Contract { Id = 1, RegistrationNumber = "C-2026-001", Institution = "ČSOB", ClientId = 1, ManagerAdvisorId = 1, DateClosed = new DateTime(2026, 1, 15), DateValidFrom = new DateTime(2026, 1, 16), DateEnded = new DateTime(2027, 1, 15) },
                new Contract { Id = 2, RegistrationNumber = "C-2026-002", Institution = "AEGON", ClientId = 2, ManagerAdvisorId = 2, DateClosed = new DateTime(2026, 2, 10), DateValidFrom = new DateTime(2026, 2, 11), DateEnded = new DateTime(2026, 8, 11) },
                new Contract { Id = 3, RegistrationNumber = "C-2026-003", Institution = "AXA", ClientId = 1, ManagerAdvisorId = 3, DateClosed = new DateTime(2026, 3, 5), DateValidFrom = new DateTime(2026, 3, 6), DateEnded = null }
            );

            modelBuilder.Entity<ContractAdvisor>().HasData(
                // Contract 1 participants: manager (1) and advisor 2
                new ContractAdvisor { ContractId = 1, AdvisorId = 1 },
                new ContractAdvisor { ContractId = 1, AdvisorId = 2 },
                // Contract 2 participants: manager (2)
                new ContractAdvisor { ContractId = 2, AdvisorId = 2 },
                // Contract 3 participants: manager (3) and advisor 1
                new ContractAdvisor { ContractId = 3, AdvisorId = 3 },
                new ContractAdvisor { ContractId = 3, AdvisorId = 1 }
            );
        }
    }
}
