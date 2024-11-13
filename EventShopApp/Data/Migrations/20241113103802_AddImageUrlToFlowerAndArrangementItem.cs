using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToFlowerAndArrangementItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlowerImageUrl",
                table: "Flowers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArrangementItemImageUrl",
                table: "ArrangementItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlowerImageUrl",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "ArrangementItemImageUrl",
                table: "ArrangementItems");
        }
    }
}
