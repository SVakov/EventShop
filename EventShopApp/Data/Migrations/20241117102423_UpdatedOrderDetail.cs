using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Flowers_FlowerId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "OrderedFlowerQuantity",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FlowerId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OrderedArrangementQuantity",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 1,
                column: "FlowerImageUrl",
                value: "https://www.google.com/search?sca_esv=4804c780a61df574&rlz=1C1CSMH_deBG1019BG1019&sxsrf=ADLYWILL-fEBWMKJ8DEd93sgIi-ZMaaubw:1731505504049&q=rose&udm=2&fbs=AEQNm0D0mdjV9iZmrIToWZfLy6hjiHLZlz0gO0cW40eqjD3LgTC_9I288s3dQhxfUDXs5Fh64FGxavo5glsqTygQ17zo5u5z-gmkJwHk96CuJucXHmdluPwYGcIpyynasv9IftnWJq-CfxpS_cad0RJd64zY0_BoK5ArRwSPBg01jRrMOCRHwSALX6-XKMwhPRWNubgHCdfCPqfrmwSM-EXYGxVfKhnPsPbd-f0c-EuCDsO_bpwPW8w&sa=X&ved=2ahUKEwiB9PrTuNmJAxVLS_EDHeVdDX0QtKgLegQIERAB&biw=1920&bih=911&dpr=1#vhid=7P8wcguiNtDg_M&vssid=mosaic");

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 2,
                column: "FlowerImageUrl",
                value: "https://s3.amazonaws.com/cdn.tulips.com/images/large/Timeless-Tulip.jpg");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Flowers_FlowerId",
                table: "OrderDetails",
                column: "FlowerId",
                principalTable: "Flowers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Flowers_FlowerId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderedArrangementQuantity",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "OrderedFlowerQuantity",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FlowerId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 1,
                column: "FlowerImageUrl",
                value: "https://example.com/images/rose.jpg");

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 2,
                column: "FlowerImageUrl",
                value: "https://example.com/images/tulip.jpg");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Flowers_FlowerId",
                table: "OrderDetails",
                column: "FlowerId",
                principalTable: "Flowers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
