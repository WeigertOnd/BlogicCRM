using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogicCRM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advisors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advisors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ManagerAdvisorId = table.Column<int>(type: "int", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnded = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Advisors_ManagerAdvisorId",
                        column: x => x.ManagerAdvisorId,
                        principalTable: "Advisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractAdvisors",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    AdvisorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAdvisors", x => new { x.ContractId, x.AdvisorId });
                    table.ForeignKey(
                        name: "FK_ContractAdvisors_Advisors_AdvisorId",
                        column: x => x.AdvisorId,
                        principalTable: "Advisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractAdvisors_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Advisors",
                columns: new[] { "Id", "Age", "BirthNumber", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, 49, "750303/1111", "anna.kralova@firma.cz", "Anna", "Králová", "+420111222333" },
                    { 2, 39, "850404/2222", "martin.dvorak@firma.cz", "Martin", "Dvořák", "+420444555666" },
                    { 3, 41, "820505/3333", "lucie.novotna@firma.cz", "Lucie", "Novotná", "+420777888999" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Age", "BirthNumber", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, 44, "800101/1234", "jan.novak@priklad.cz", "Jan", "Novák", "+420123456789" },
                    { 2, 34, "900202/4321", "petra.svobodova@priklad.cz", "Petra", "Svobodová", "+420987654321" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ClientId", "DateClosed", "DateEnded", "DateValidFrom", "Institution", "ManagerAdvisorId", "RegistrationNumber" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "ČSOB", 1, "C-2026-001" },
                    { 2, 2, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "AEGON", 2, "C-2026-002" },
                    { 3, 1, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "AXA", 3, "C-2026-003" }
                });

            migrationBuilder.InsertData(
                table: "ContractAdvisors",
                columns: new[] { "AdvisorId", "ContractId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 1, 3 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractAdvisors_AdvisorId",
                table: "ContractAdvisors",
                column: "AdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ManagerAdvisorId",
                table: "Contracts",
                column: "ManagerAdvisorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractAdvisors");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Advisors");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
