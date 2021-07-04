using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceIt.Data.Migrations
{
    public partial class FixListAndListitemTableBinding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_UserLists_UserListId",
                table: "ListItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserListId",
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
                value: new DateTime(2021, 6, 28, 20, 22, 31, 658, DateTimeKind.Local).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 20, 22, 31, 662, DateTimeKind.Local).AddTicks(3352));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 28, 20, 22, 31, 662, DateTimeKind.Local).AddTicks(3474));

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_UserLists_UserListId",
                table: "ListItems",
                column: "UserListId",
                principalTable: "UserLists",
                principalColumn: "UserListId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_UserLists_UserListId",
                table: "ListItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserListId",
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
                value: new DateTime(2021, 6, 26, 19, 25, 25, 541, DateTimeKind.Local).AddTicks(8927));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 26, 19, 25, 25, 544, DateTimeKind.Local).AddTicks(5066));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 26, 19, 25, 25, 544, DateTimeKind.Local).AddTicks(5118));

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_UserLists_UserListId",
                table: "ListItems",
                column: "UserListId",
                principalTable: "UserLists",
                principalColumn: "UserListId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
