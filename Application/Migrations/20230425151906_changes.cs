using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImages_AppProperties_propertiesId",
                table: "PropertyImages");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImages_propertiesId",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "propertiesId",
                table: "PropertyImages");

            migrationBuilder.AddColumn<string>(
                name: "SaleId",
                table: "PropertyImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_propertyId",
                table: "PropertyImages",
                column: "propertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImages_AppProperties_propertyId",
                table: "PropertyImages",
                column: "propertyId",
                principalTable: "AppProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImages_AppProperties_propertyId",
                table: "PropertyImages");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImages_propertyId",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "PropertyImages");

            migrationBuilder.AddColumn<int>(
                name: "propertiesId",
                table: "PropertyImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_propertiesId",
                table: "PropertyImages",
                column: "propertiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImages_AppProperties_propertiesId",
                table: "PropertyImages",
                column: "propertiesId",
                principalTable: "AppProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
