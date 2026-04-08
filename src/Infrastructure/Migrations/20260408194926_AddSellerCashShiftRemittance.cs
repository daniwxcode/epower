using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorHero.CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerCashShiftRemittance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashShiftId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MaxCreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashShifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpectedBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashShifts_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashRemittances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashShiftId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceivedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRemittances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashRemittances_CashShifts_CashShiftId",
                        column: x => x.CashShiftId,
                        principalTable: "CashShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CashShiftId",
                table: "Payments",
                column: "CashShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRemittances_CashShiftId",
                table: "CashRemittances",
                column: "CashShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_CashShifts_SellerId",
                table: "CashShifts",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CashShifts_CashShiftId",
                table: "Payments",
                column: "CashShiftId",
                principalTable: "CashShifts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CashShifts_CashShiftId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CashRemittances");

            migrationBuilder.DropTable(
                name: "CashShifts");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CashShiftId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CashShiftId",
                table: "Payments");
        }
    }
}
