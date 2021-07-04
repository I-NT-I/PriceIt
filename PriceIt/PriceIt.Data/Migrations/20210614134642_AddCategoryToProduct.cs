using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceIt.Data.Migrations
{
    public partial class AddCategoryToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 14, 15, 46, 41, 532, DateTimeKind.Local).AddTicks(466));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 14, 15, 46, 41, 536, DateTimeKind.Local).AddTicks(1297));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 14, 15, 46, 41, 536, DateTimeKind.Local).AddTicks(1376));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 10, 9, 41, 51, 556, DateTimeKind.Local).AddTicks(8283));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 10, 9, 41, 51, 559, DateTimeKind.Local).AddTicks(7110));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 10, 9, 41, 51, 559, DateTimeKind.Local).AddTicks(7157));
        }
    }
}
