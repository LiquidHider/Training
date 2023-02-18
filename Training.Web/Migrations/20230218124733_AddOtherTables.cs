using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Web.Migrations
{
    public partial class AddOtherTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisteredInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StorageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredInvoices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredInvoices_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sells_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Сount = table.Column<int>(type: "int", nullable: false),
                    SellId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Sells_SellId",
                        column: x => x.SellId,
                        principalTable: "Sells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredInvoices_ClientId",
                table: "RegisteredInvoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredInvoices_GoodId",
                table: "RegisteredInvoices",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_SellId",
                table: "SalesInvoices",
                column: "SellId");

            migrationBuilder.CreateIndex(
                name: "IX_Sells_GoodId",
                table: "Sells",
                column: "GoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisteredInvoices");

            migrationBuilder.DropTable(
                name: "SalesInvoices");

            migrationBuilder.DropTable(
                name: "Sells");
        }
    }
}
