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
                new Client { Id = 1, FirstName = "Jan", LastName = "Novák", Email = "jan.novak@example.com", Phone = "+420123456789", BirthNumber = "800101/1234", Age = 44 },
                new Client { Id = 2, FirstName = "Petr", LastName = "Svoboda", Email = "petr.svoboda@example.com", Phone = "+420987654321", BirthNumber = "900202/4321", Age = 34 }
            );

            modelBuilder.Entity<Advisor>().HasData(
                new Advisor { Id = 1, FirstName = "Anna", LastName = "Kralova", Email = "anna.kralova@example.com", Phone = "+420111222333", BirthNumber = "750303/1111", Age = 49 },
                new Advisor { Id = 2, FirstName = "Martin", LastName = "Dvorak", Email = "martin.dvorak@example.com", Phone = "+420444555666", BirthNumber = "850404/2222", Age = 39 }
            );

            modelBuilder.Entity<Contract>().HasData(
                new Contract { Id = 1, RegistrationNumber = "C-2026-001", Institution = "Acme Corp", ClientId = 1, ManagerAdvisorId = 1, DateClosed = DateTime.UtcNow.AddDays(-30), DateValidFrom = DateTime.UtcNow.AddDays(-29), DateEnded = DateTime.UtcNow.AddYears(1) },
                new Contract { Id = 2, RegistrationNumber = "C-2026-002", Institution = "Beta Ltd", ClientId = 2, ManagerAdvisorId = 2, DateClosed = DateTime.UtcNow.AddDays(-10), DateValidFrom = DateTime.UtcNow.AddDays(-9), DateEnded = DateTime.UtcNow.AddMonths(6) }
            );

            modelBuilder.Entity<ContractAdvisor>().HasData(
                new ContractAdvisor { ContractId = 1, AdvisorId = 1 },
                new ContractAdvisor { ContractId = 1, AdvisorId = 2 },
                new ContractAdvisor { ContractId = 2, AdvisorId = 2 }
            );
        }
    }
}
