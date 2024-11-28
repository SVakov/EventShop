using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBackingFieldToIsAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Flowers",
                type: "bit",
                nullable: false,
                defaultValue: false);
  
            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsAvailable",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Flowers");
        }
    }
}
