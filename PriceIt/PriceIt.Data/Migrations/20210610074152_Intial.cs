using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceIt.Data.Migrations
{
    public partial class Intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    ProductUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "LastUpdate", "Name", "Price", "ProductImageUrl", "ProductUrl", "Website" },
                values: new object[] { 1, new DateTime(2021, 6, 10, 9, 41, 51, 556, DateTimeKind.Local).AddTicks(8283), "TestProduct1", 60.99f, "https://assets.mmsrg.com/isr/166325/c1/-/ASSET_MMS_72715345/fee_786_587_png", "https://www.saturn.de/de/product/_cooler-master-masterwatt-lite-700w-230v-2602309.html", 0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "LastUpdate", "Name", "Price", "ProductImageUrl", "ProductUrl", "Website" },
                values: new object[] { 2, new DateTime(2021, 6, 10, 9, 41, 51, 559, DateTimeKind.Local).AddTicks(7110), "TestProduct2", 60.99f, "https://assets.mmsrg.com/isr/166325/c1/-/ASSET_MMS_72796414/fee_786_587_png", "https://www.saturn.de/de/product/_seasonic-core-gc-650-gold-2625115.html", 0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "LastUpdate", "Name", "Price", "ProductImageUrl", "ProductUrl", "Website" },
                values: new object[] { 3, new DateTime(2021, 6, 10, 9, 41, 51, 559, DateTimeKind.Local).AddTicks(7157), "TestProduct3", 60.99f, "https://assets.mmsrg.com/isr/166325/c1/-/pixelboxx-mss-80864028/fee_786_587_png", "https://www.saturn.de/de/product/_be-quiet-pure-power-11-500w-2505555.html", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
