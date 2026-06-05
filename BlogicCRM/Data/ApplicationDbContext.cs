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
        public DbSet<BlogicCRM.Models.UserAccount> UserAccounts => Set<BlogicCRM.Models.UserAccount>();

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

            // Seed data (expanded)
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, FirstName = "Jan", LastName = "Novák", Email = "jan.novak@priklad.cz", Phone = "+420123456789", BirthNumber = "800101/1234", Age = 44 },
                new Client { Id = 2, FirstName = "Petra", LastName = "Svobodová", Email = "petra.svobodova@priklad.cz", Phone = "+420987654321", BirthNumber = "900202/4321", Age = 34 },
                new Client { Id = 3, FirstName = "Tomáš", LastName = "Horák", Email = "tomas.horak@priklad.cz", Phone = "+420601000001", BirthNumber = "880303/1111", Age = 38 },
                new Client { Id = 4, FirstName = "Kateřina", LastName = "Malá", Email = "katerina.mala@priklad.cz", Phone = "+420602000002", BirthNumber = "910404/2222", Age = 35 },
                new Client { Id = 5, FirstName = "Michal", LastName = "Beneš", Email = "michal.benes@priklad.cz", Phone = "+420603000003", BirthNumber = "750505/3333", Age = 51 },
                new Client { Id = 6, FirstName = "Jana", LastName = "Pokorná", Email = "jana.pokorna@priklad.cz", Phone = "+420604000004", BirthNumber = "990606/4444", Age = 27 },
                new Client { Id = 7, FirstName = "Pavel", LastName = "Černý", Email = "pavel.cerny@priklad.cz", Phone = "+420605000005", BirthNumber = "650707/5555", Age = 61 },
                new Client { Id = 8, FirstName = "Eva", LastName = "Králíková", Email = "eva.kralikova@priklad.cz", Phone = "+420606000006", BirthNumber = "900808/6666", Age = 36 }
            );

            modelBuilder.Entity<Advisor>().HasData(
                new Advisor { Id = 1, FirstName = "Anna", LastName = "Králová", Email = "anna.kralova@firma.cz", Phone = "+420111222333", BirthNumber = "750303/1111", Age = 49 },
                new Advisor { Id = 2, FirstName = "Martin", LastName = "Dvořák", Email = "martin.dvorak@firma.cz", Phone = "+420444555666", BirthNumber = "850404/2222", Age = 39 },
                new Advisor { Id = 3, FirstName = "Lucie", LastName = "Novotná", Email = "lucie.novotna@firma.cz", Phone = "+420777888999", BirthNumber = "820505/3333", Age = 41 },
                new Advisor { Id = 4, FirstName = "Petr", LastName = "Svoboda", Email = "petr.svoboda@firma.cz", Phone = "+420201000111", BirthNumber = "800808/1212", Age = 46 },
                new Advisor { Id = 5, FirstName = "Hana", LastName = "Bartošová", Email = "hana.bartosova@firma.cz", Phone = "+420202000222", BirthNumber = "850909/2323", Age = 41 },
                new Advisor { Id = 6, FirstName = "Robert", LastName = "Mašek", Email = "robert.masek@firma.cz", Phone = "+420203000333", BirthNumber = "780101/3434", Age = 48 },
                new Advisor { Id = 7, FirstName = "Silvia", LastName = "Urban", Email = "silvia.urban@firma.cz", Phone = "+420204000444", BirthNumber = "920202/4545", Age = 34 },
                new Advisor { Id = 8, FirstName = "Ondřej", LastName = "Holub", Email = "ondrej.holub@firma.cz", Phone = "+420205000555", BirthNumber = "950303/5656", Age = 31 }
            );

            modelBuilder.Entity<Contract>().HasData(
                new Contract { Id = 1, RegistrationNumber = "C-2026-001", Institution = "ČSOB", ClientId = 1, ManagerAdvisorId = 1, DateClosed = new DateTime(2026, 1, 15), DateValidFrom = new DateTime(2026, 1, 16), DateEnded = new DateTime(2027, 1, 15) },
                new Contract { Id = 2, RegistrationNumber = "C-2026-002", Institution = "AEGON", ClientId = 2, ManagerAdvisorId = 2, DateClosed = new DateTime(2026, 2, 10), DateValidFrom = new DateTime(2026, 2, 11), DateEnded = new DateTime(2026, 8, 11) },
                new Contract { Id = 3, RegistrationNumber = "C-2026-003", Institution = "AXA", ClientId = 1, ManagerAdvisorId = 3, DateClosed = new DateTime(2026, 3, 5), DateValidFrom = new DateTime(2026, 3, 6), DateEnded = null },
                new Contract { Id = 4, RegistrationNumber = "C-2026-004", Institution = "Allianz", ClientId = 3, ManagerAdvisorId = 4, DateClosed = new DateTime(2026, 1, 10), DateValidFrom = new DateTime(2026, 1, 15), DateEnded = null },
                new Contract { Id = 5, RegistrationNumber = "C-2026-005", Institution = "Generali", ClientId = 4, ManagerAdvisorId = 5, DateClosed = new DateTime(2026, 2, 12), DateValidFrom = new DateTime(2026, 2, 15), DateEnded = new DateTime(2026, 9, 1) },
                new Contract { Id = 6, RegistrationNumber = "C-2026-006", Institution = "Komerční banka", ClientId = 5, ManagerAdvisorId = 6, DateClosed = new DateTime(2026, 3, 1), DateValidFrom = new DateTime(2026, 3, 5), DateEnded = null },
                new Contract { Id = 7, RegistrationNumber = "C-2026-007", Institution = "Česká spořitelna", ClientId = 6, ManagerAdvisorId = 7, DateClosed = new DateTime(2025, 12, 1), DateValidFrom = new DateTime(2026, 1, 1), DateEnded = new DateTime(2026, 6, 30) },
                new Contract { Id = 8, RegistrationNumber = "C-2026-008", Institution = "UniCredit", ClientId = 7, ManagerAdvisorId = 8, DateClosed = new DateTime(2026, 4, 20), DateValidFrom = new DateTime(2026, 4, 21), DateEnded = null },
                new Contract { Id = 9, RegistrationNumber = "C-2026-009", Institution = "AXA", ClientId = 8, ManagerAdvisorId = 1, DateClosed = new DateTime(2026, 5, 5), DateValidFrom = new DateTime(2026, 5, 6), DateEnded = new DateTime(2026, 5, 31) },
                new Contract { Id = 10, RegistrationNumber = "C-2026-010", Institution = "Kooperativa", ClientId = 1, ManagerAdvisorId = 2, DateClosed = new DateTime(2026, 6, 10), DateValidFrom = new DateTime(2026, 6, 15), DateEnded = new DateTime(2027, 6, 14) },
                new Contract { Id = 11, RegistrationNumber = "C-2026-011", Institution = "ČSOB", ClientId = 2, ManagerAdvisorId = 3, DateClosed = new DateTime(2026, 7, 1), DateValidFrom = new DateTime(2026, 7, 2), DateEnded = null },
                new Contract { Id = 12, RegistrationNumber = "C-2026-012", Institution = "Generali", ClientId = 3, ManagerAdvisorId = 4, DateClosed = new DateTime(2026, 8, 1), DateValidFrom = new DateTime(2026, 8, 2), DateEnded = new DateTime(2026, 8, 15) },
                new Contract { Id = 13, RegistrationNumber = "C-2026-013", Institution = "Allianz", ClientId = 4, ManagerAdvisorId = 5, DateClosed = new DateTime(2026, 9, 1), DateValidFrom = new DateTime(2026, 9, 10), DateEnded = null },
                new Contract { Id = 14, RegistrationNumber = "C-2026-014", Institution = "AEGON", ClientId = 5, ManagerAdvisorId = 6, DateClosed = new DateTime(2026, 1, 20), DateValidFrom = new DateTime(2026, 2, 1), DateEnded = new DateTime(2026, 2, 28) },
                new Contract { Id = 15, RegistrationNumber = "C-2026-015", Institution = "Komerční banka", ClientId = 6, ManagerAdvisorId = 7, DateClosed = new DateTime(2026, 11, 1), DateValidFrom = new DateTime(2026, 11, 2), DateEnded = null }
            );

            modelBuilder.Entity<ContractAdvisor>().HasData(
                // Contract 1 participants
                new ContractAdvisor { ContractId = 1, AdvisorId = 1 },
                new ContractAdvisor { ContractId = 1, AdvisorId = 2 },
                // Contract 2 participants
                new ContractAdvisor { ContractId = 2, AdvisorId = 2 },
                // Contract 3 participants
                new ContractAdvisor { ContractId = 3, AdvisorId = 3 },
                new ContractAdvisor { ContractId = 3, AdvisorId = 1 },
                // Contract 4
                new ContractAdvisor { ContractId = 4, AdvisorId = 4 },
                new ContractAdvisor { ContractId = 4, AdvisorId = 1 },
                // Contract 5
                new ContractAdvisor { ContractId = 5, AdvisorId = 5 },
                new ContractAdvisor { ContractId = 5, AdvisorId = 2 },
                new ContractAdvisor { ContractId = 5, AdvisorId = 3 },
                // Contract 6
                new ContractAdvisor { ContractId = 6, AdvisorId = 6 },
                // Contract 7
                new ContractAdvisor { ContractId = 7, AdvisorId = 7 },
                new ContractAdvisor { ContractId = 7, AdvisorId = 4 },
                // Contract 8
                new ContractAdvisor { ContractId = 8, AdvisorId = 8 },
                new ContractAdvisor { ContractId = 8, AdvisorId = 5 },
                new ContractAdvisor { ContractId = 8, AdvisorId = 6 },
                // Contract 9
                new ContractAdvisor { ContractId = 9, AdvisorId = 1 },
                new ContractAdvisor { ContractId = 9, AdvisorId = 2 },
                // Contract 10
                new ContractAdvisor { ContractId = 10, AdvisorId = 2 },
                new ContractAdvisor { ContractId = 10, AdvisorId = 3 },
                // Contract 11
                new ContractAdvisor { ContractId = 11, AdvisorId = 3 },
                new ContractAdvisor { ContractId = 11, AdvisorId = 1 },
                // Contract 12
                new ContractAdvisor { ContractId = 12, AdvisorId = 4 },
                // Contract 13
                new ContractAdvisor { ContractId = 13, AdvisorId = 5 },
                new ContractAdvisor { ContractId = 13, AdvisorId = 2 },
                // Contract 14
                new ContractAdvisor { ContractId = 14, AdvisorId = 6 },
                // Contract 15
                new ContractAdvisor { ContractId = 15, AdvisorId = 7 },
                new ContractAdvisor { ContractId = 15, AdvisorId = 8 }
            );

            // UserAccounts table
            modelBuilder.Entity<BlogicCRM.Models.UserAccount>(eb =>
            {
                eb.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
