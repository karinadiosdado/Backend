using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CambioenAlcohol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beers_Brands_BranID",
                table: "Beers");

            migrationBuilder.DropIndex(
                name: "IX_Beers_BranID",
                table: "Beers");

            migrationBuilder.DropColumn(
                name: "BranID",
                table: "Beers");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Brands",
                newName: "Name");

            migrationBuilder.AddColumn<decimal>(
                name: "Alcohol",
                table: "Beers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Beers_BrandId",
                table: "Beers",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beers_Brands_BrandId",
                table: "Beers",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beers_Brands_BrandId",
                table: "Beers");

            migrationBuilder.DropIndex(
                name: "IX_Beers_BrandId",
                table: "Beers");

            migrationBuilder.DropColumn(
                name: "Alcohol",
                table: "Beers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Brands",
                newName: "name");

            migrationBuilder.AddColumn<int>(
                name: "BranID",
                table: "Beers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Beers_BranID",
                table: "Beers",
                column: "BranID");

            migrationBuilder.AddForeignKey(
                name: "FK_Beers_Brands_BranID",
                table: "Beers",
                column: "BranID",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
