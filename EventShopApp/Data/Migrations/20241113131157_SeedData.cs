using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ArrangementItems",
                columns: new[] { "Id", "ArrangementItemImageUrl", "ArrangementItemType", "ArrangementItemsQuantity", "Description", "Price" },
                values: new object[,]
                {
                    { 1, "https://example.com/images/birthday_bouquet.jpg", "Birthday Bouquet", 20, "A colorful bouquet perfect for birthdays.", 19.99m },
                    { 2, "https://example.com/images/wedding_arrangement.jpg", "Wedding Arrangement", 15, "Elegant flowers for weddings.", 29.99m }
                });

            migrationBuilder.InsertData(
                table: "Flowers",
                columns: new[] { "Id", "Description", "FlowerImageUrl", "FlowerQuantity", "FlowerType", "Price" },
                values: new object[,]
                {
                    { 1, "A beautiful red rose.", "https://example.com/images/rose.jpg", 100, "Rose", 2.99m },
                    { 2, "A charming yellow tulip.", "https://example.com/images/tulip.jpg", 50, "Tulip", 1.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArrangementItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ArrangementItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
