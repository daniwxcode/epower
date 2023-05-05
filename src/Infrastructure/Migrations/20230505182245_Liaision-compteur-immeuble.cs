using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorHero.CleanArchitecture.Infrastructure.Migrations
{
    public partial class Liaisioncompteurimmeuble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Meters_InternalMeterId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_InternalMeterId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "InternalMeterId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Meters");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Meters",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Meters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Meters_BuildingId",
                table: "Meters",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_Buildings_BuildingId",
                table: "Meters",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meters_Buildings_BuildingId",
                table: "Meters");

            migrationBuilder.DropIndex(
                name: "IX_Meters_BuildingId",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Meters");

            migrationBuilder.AddColumn<int>(
                name: "InternalMeterId",
                table: "Shops",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Meters",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Meters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_InternalMeterId",
                table: "Shops",
                column: "InternalMeterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Meters_InternalMeterId",
                table: "Shops",
                column: "InternalMeterId",
                principalTable: "Meters",
                principalColumn: "Id");
        }
    }
}
