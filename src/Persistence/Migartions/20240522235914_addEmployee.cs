using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    NormalDailyWorkingHours = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaryRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SalaryPerHr = table.Column<double>(type: "double precision", nullable: false),
                    OvertimeSalaryPerHr = table.Column<double>(type: "double precision", nullable: false),
                    ToDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EmployeeEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaryRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryRecord_Employees_EmployeeEntityId",
                        column: x => x.EmployeeEntityId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorkShift",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    BreakMinutes = table.Column<int>(type: "integer", nullable: false),
                    EmployeeEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkShift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkShift_Employees_EmployeeEntityId",
                        column: x => x.EmployeeEntityId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryRecord_EmployeeEntityId",
                table: "EmployeeSalaryRecord",
                column: "EmployeeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkShift_EmployeeEntityId",
                table: "EmployeeWorkShift",
                column: "EmployeeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkShifts_EmployeeId_Date",
                table: "EmployeeWorkShift",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Employees",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSalaryRecord");

            migrationBuilder.DropTable(
                name: "EmployeeWorkShift");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
