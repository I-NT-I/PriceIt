using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceIt.Data.Migrations
{
    public partial class FixProductIdTableBinding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_Products_ProductId",
                table: "ListItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ListItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 21, 6, 9, 515, DateTimeKind.Local).AddTicks(8269));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 21, 6, 9, 518, DateTimeKind.Local).AddTicks(7091));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 21, 6, 9, 518, DateTimeKind.Local).AddTicks(7139));

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_Products_ProductId",
                table: "ListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_Products_ProductId",
                table: "ListItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ListItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 20, 57, 58, 873, DateTimeKind.Local).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 20, 57, 58, 876, DateTimeKind.Local).AddTicks(5209));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 20, 57, 58, 876, DateTimeKind.Local).AddTicks(5276));

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_Products_ProductId",
                table: "ListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
