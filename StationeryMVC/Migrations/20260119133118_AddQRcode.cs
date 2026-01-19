using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StationeryMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddQRcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QRCodeImagePath",
                table: "StationeryItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRCodeImagePath",
                table: "StationeryItems");
        }
    }
}
