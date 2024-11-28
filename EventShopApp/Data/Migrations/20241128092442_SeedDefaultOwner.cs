using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "HireDate", "IsFired", "Name", "PhoneNumber", "Role", "Surname" },
                values: new object[] { 1, "vakovslavcho@gmail.com", new DateTime(2024, 11, 28, 9, 24, 40, 721, DateTimeKind.Utc).AddTicks(3943), false, "Slavcho", "+359893540139", 0, "Vakov" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            
        }
    }
}
