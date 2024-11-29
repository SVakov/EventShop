using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArrangementItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "ArrangementItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ArrangementItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "ArrangementItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsAvailable",
                value: false);

            migrationBuilder.Sql(@"
                UPDATE ArrangementItems
                SET IsAvailable = CASE 
                    WHEN ArrangementItemsQuantity > 0 THEN 1
                    ELSE 0
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "ArrangementItems");
        }
    }
}
