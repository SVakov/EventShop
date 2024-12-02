using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAvailabilityLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Flowers SET IsAvailable = CASE WHEN FlowerQuantity > 0 THEN 1 ELSE 0 END");
            migrationBuilder.Sql("UPDATE ArrangementItems SET IsAvailable = CASE WHEN ArrangementItemsQuantity > 0 THEN 1 ELSE 0 END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Flowers SET IsAvailable = NULL");
            migrationBuilder.Sql("UPDATE ArrangementItems SET IsAvailable = NULL");
        }
    }
}
