using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace BlogicCRM.Migrations
{
    public partial class RozsireniTestovacichDat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Advisors",
                columns: new[] { "Id", "Age", "BirthNumber", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 4, 46, "800808/1212", "petr.svoboda@firma.cz", "Petr", "Svoboda", "+420201000111" },
                    { 5, 41, "850909/2323", "hana.bartosova@firma.cz", "Hana", "Bartošová", "+420202000222" },
                    { 6, 48, "780101/3434", "robert.masek@firma.cz", "Robert", "Mašek", "+420203000333" },
                    { 7, 34, "920202/4545", "silvia.urban@firma.cz", "Silvia", "Urban", "+420204000444" },
                    { 8, 31, "950303/5656", "ondrej.holub@firma.cz", "Ondřej", "Holub", "+420205000555" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Age", "BirthNumber", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 3, 38, "880303/1111", "tomas.horak@priklad.cz", "Tomáš", "Horák", "+420601000001" },
                    { 4, 35, "910404/2222", "katerina.mala@priklad.cz", "Kateřina", "Malá", "+420602000002" },
                    { 5, 51, "750505/3333", "michal.benes@priklad.cz", "Michal", "Beneš", "+420603000003" },
                    { 6, 27, "990606/4444", "jana.pokorna@priklad.cz", "Jana", "Pokorná", "+420604000004" },
                    { 7, 61, "650707/5555", "pavel.cerny@priklad.cz", "Pavel", "Černý", "+420605000005" },
                    { 8, 36, "900808/6666", "eva.kralikova@priklad.cz", "Eva", "Králíková", "+420606000006" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ClientId", "DateClosed", "DateEnded", "DateValidFrom", "Institution", "ManagerAdvisorId", "RegistrationNumber" },
                values: new object[,]
                {
                    { 10, 1, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kooperativa", 2, "C-2026-010" },
                    { 11, 2, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "ČSOB", 3, "C-2026-011" }
                });

            migrationBuilder.InsertData(
                table: "ContractAdvisors",
                columns: new[] { "AdvisorId", "ContractId" },
                values: new object[,]
                {
                    { 2, 10 },
                    { 3, 10 },
                    { 1, 11 },
                    { 3, 11 }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ClientId", "DateClosed", "DateEnded", "DateValidFrom", "Institution", "ManagerAdvisorId", "RegistrationNumber" },
                values: new object[,]
                {
                    { 4, 3, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Allianz", 4, "C-2026-004" },
                    { 5, 4, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Generali", 5, "C-2026-005" },
                    { 6, 5, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Komerční banka", 6, "C-2026-006" },
                    { 7, 6, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Česká spořitelna", 7, "C-2026-007" },
                    { 8, 7, new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "UniCredit", 8, "C-2026-008" },
                    { 9, 8, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "AXA", 1, "C-2026-009" },
                    { 12, 3, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Generali", 4, "C-2026-012" },
                    { 13, 4, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Allianz", 5, "C-2026-013" },
                    { 14, 5, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AEGON", 6, "C-2026-014" },
                    { 15, 6, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Komerční banka", 7, "C-2026-015" }
                });

            migrationBuilder.InsertData(
                table: "ContractAdvisors",
                columns: new[] { "AdvisorId", "ContractId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 4, 4 },
                    { 2, 5 },
                    { 3, 5 },
                    { 5, 5 },
                    { 6, 6 },
                    { 4, 7 },
                    { 7, 7 },
                    { 5, 8 },
                    { 6, 8 },
                    { 8, 8 },
                    { 1, 9 },
                    { 2, 9 },
                    { 4, 12 },
                    { 2, 13 },
                    { 5, 13 },
                    { 6, 14 },
                    { 7, 15 },
                    { 8, 15 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 4, 7 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 5, 8 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 6, 8 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 8, 8 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 2, 9 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 2, 10 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 3, 10 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 3, 11 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 4, 12 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 2, 13 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 5, 13 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 6, 14 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 7, 15 });

            migrationBuilder.DeleteData(
                table: "ContractAdvisors",
                keyColumns: new[] { "AdvisorId", "ContractId" },
                keyValues: new object[] { 8, 15 });

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Advisors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Advisors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Advisors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Advisors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Advisors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
