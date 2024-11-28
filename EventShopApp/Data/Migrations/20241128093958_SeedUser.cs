using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a1df175b-7510-4197-9334-08a434c5bd4f", 0, "28b4fac8-89dd-4abd-a8da-0b3d96f839ea", "vakovslavcho@gmail.com", true, false, null, "VAKOVSLAVCHO@GMAIL.COM", "VAKOVSLAVCHO@GMAIL.COM", "AQAAAAIAAYagAAAAEEfP1uRCs2wFSUm1jAjApW4Nr8GPgDFrKWd+kHuEcbxyZvVvm+k58qsu4vZ3SvpQzA==", null, false, "411a7afe-831f-4818-90ee-ecca67f7e89a", false, "vakovslavcho@gmail.com" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "HireDate",
                value: new DateTime(2024, 11, 28, 9, 39, 53, 566, DateTimeKind.Utc).AddTicks(7407));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1df175b-7510-4197-9334-08a434c5bd4f");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "HireDate",
                value: new DateTime(2024, 11, 28, 9, 24, 40, 721, DateTimeKind.Utc).AddTicks(3943));
        }
    }
}
