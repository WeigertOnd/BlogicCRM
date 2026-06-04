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
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BirthNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true)
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
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BirthNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true)
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
                    Institution = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ManagerAdvisorId = table.Column<int>(type: "int", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    { 1, 49, "750303/1111", "anna.kralova@example.com", "Anna", "Kralova", "+420111222333" },
                    { 2, 39, "850404/2222", "martin.dvorak@example.com", "Martin", "Dvorak", "+420444555666" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Age", "BirthNumber", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, 44, "800101/1234", "jan.novak@example.com", "Jan", "Novák", "+420123456789" },
                    { 2, 34, "900202/4321", "petr.svoboda@example.com", "Petr", "Svoboda", "+420987654321" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ClientId", "DateClosed", "DateEnded", "DateValidFrom", "Institution", "ManagerAdvisorId", "RegistrationNumber" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 5, 5, 10, 52, 0, 847, DateTimeKind.Utc).AddTicks(7612), new DateTime(2027, 6, 4, 10, 52, 0, 847, DateTimeKind.Utc).AddTicks(7620), new DateTime(2026, 5, 6, 10, 52, 0, 847, DateTimeKind.Utc).AddTicks(7619), "Acme Corp", 1, "C-2026-001" },
                    { 2, 2, new DateTime(2026, 5, 25, 10, 52, 0, 847, DateTimeKind.Utc).AddTicks(7649), new DateTime(2026, 12, 4, 10, 52, 0, 847, DateTimeKind.Utc).AddTicks(7651), new DateTime(2026, 5, 26, 10, 52, 0, 847, DateTimeKind.Utc).AddTicks(7650), "Beta Ltd", 2, "C-2026-002" }
                });

            migrationBuilder.InsertData(
                table: "ContractAdvisors",
                columns: new[] { "AdvisorId", "ContractId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 }
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
