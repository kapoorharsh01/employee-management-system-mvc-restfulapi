using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllEmployees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllEmployees", x => x.EmployeeId);
                });

            migrationBuilder.InsertData(
                table: "AllEmployees",
                columns: new[] { "EmployeeId", "DateOfJoining", "Department", "Email", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technical", "harsh@test.com", "Harsh Kapoor" },
                    { 2, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "harry@test.com", "Harry" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllEmployees");
        }
    }
}
