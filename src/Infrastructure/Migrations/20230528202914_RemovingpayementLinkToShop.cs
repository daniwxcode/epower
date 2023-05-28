using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorHero.CleanArchitecture.Infrastructure.Migrations
{
    public partial class RemovingpayementLinkToShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Shops_ShopId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ShopId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Payments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ShopId",
                table: "Payments",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Shops_ShopId",
                table: "Payments",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }
    }
}
