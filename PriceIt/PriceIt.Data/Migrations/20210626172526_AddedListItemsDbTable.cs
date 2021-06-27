using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceIt.Data.Migrations
{
    public partial class AddedListItemsDbTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItem_Products_ProductId",
                table: "ListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ListItem_UserLists_UserListId",
                table: "ListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListItem",
                table: "ListItem");

            migrationBuilder.RenameTable(
                name: "ListItem",
                newName: "ListItems");

            migrationBuilder.RenameIndex(
                name: "IX_ListItem_UserListId",
                table: "ListItems",
                newName: "IX_ListItems_UserListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListItem_ProductId",
                table: "ListItems",
                newName: "IX_ListItems_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListItems",
                table: "ListItems",
                column: "ListItemId");

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
                name: "FK_ListItems_Products_ProductId",
                table: "ListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_UserLists_UserListId",
                table: "ListItems",
                column: "UserListId",
                principalTable: "UserLists",
                principalColumn: "UserListId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_Products_ProductId",
                table: "ListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_UserLists_UserListId",
                table: "ListItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListItems",
                table: "ListItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ListItems");

            migrationBuilder.RenameTable(
                name: "ListItems",
                newName: "ListItem");

            migrationBuilder.RenameIndex(
                name: "IX_ListItems_UserListId",
                table: "ListItem",
                newName: "IX_ListItem_UserListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListItems_ProductId",
                table: "ListItem",
                newName: "IX_ListItem_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListItem",
                table: "ListItem",
                column: "ListItemId");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 26, 14, 2, 31, 646, DateTimeKind.Local).AddTicks(9399));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 26, 14, 2, 31, 649, DateTimeKind.Local).AddTicks(5232));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 26, 14, 2, 31, 649, DateTimeKind.Local).AddTicks(5276));

            migrationBuilder.AddForeignKey(
                name: "FK_ListItem_Products_ProductId",
                table: "ListItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListItem_UserLists_UserListId",
                table: "ListItem",
                column: "UserListId",
                principalTable: "UserLists",
                principalColumn: "UserListId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
