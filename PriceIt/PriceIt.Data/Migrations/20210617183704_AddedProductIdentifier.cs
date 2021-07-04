using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceIt.Data.Migrations
{
    public partial class AddedProductIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductIdentifier",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 17, 20, 37, 2, 814, DateTimeKind.Local).AddTicks(3033));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 17, 20, 37, 2, 818, DateTimeKind.Local).AddTicks(1009));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 17, 20, 37, 2, 818, DateTimeKind.Local).AddTicks(1057));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductIdentifier",
                table: "Products");

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
    }
}
