using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceIt.Data.Migrations
{
    public partial class AddedListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UserLists_UserListId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserListId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserListId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ListItem",
                columns: table => new
                {
                    ListItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    UserListId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItem", x => x.ListItemId);
                    table.ForeignKey(
                        name: "FK_ListItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListItem_UserLists_UserListId",
                        column: x => x.UserListId,
                        principalTable: "UserLists",
                        principalColumn: "UserListId",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ListItem_ProductId",
                table: "ListItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ListItem_UserListId",
                table: "ListItem",
                column: "UserListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListItem");

            migrationBuilder.AddColumn<int>(
                name: "UserListId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 20, 17, 23, 3, 705, DateTimeKind.Local).AddTicks(4719));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 20, 17, 23, 3, 708, DateTimeKind.Local).AddTicks(983));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2021, 6, 20, 17, 23, 3, 708, DateTimeKind.Local).AddTicks(1029));

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserListId",
                table: "Products",
                column: "UserListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UserLists_UserListId",
                table: "Products",
                column: "UserListId",
                principalTable: "UserLists",
                principalColumn: "UserListId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
