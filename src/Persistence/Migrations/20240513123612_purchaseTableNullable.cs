using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class purchaseTableNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillingParties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    VatNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingParties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentStockAmount = table.Column<double>(type: "double precision", nullable: false),
                    CurrentEstimatedStockValuePerKilo = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BillingPartyId = table.Column<Guid>(type: "uuid", nullable: false),
                    VatAmount = table.Column<double>(type: "double precision", nullable: true),
                    TransportFee = table.Column<double>(type: "double precision", nullable: true),
                    PaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    InvoiceNumber = table.Column<int>(type: "integer", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_BillingParties_BillingPartyId",
                        column: x => x.BillingPartyId,
                        principalTable: "BillingParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    ExpectedValuePerKilo = table.Column<double>(type: "double precision", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    EntryCategory = table.Column<string>(type: "text", nullable: false),
                    ItemEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Items_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseLineItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Report = table.Column<double>(type: "double precision", nullable: false),
                    PurchaseEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseLineItem_Items_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseLineItem_Purchases_PurchaseEntityId",
                        column: x => x.PurchaseEntityId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillingParties_Email",
                table: "BillingParties",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillingParties_Name",
                table: "BillingParties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillingParties_VatNumber",
                table: "BillingParties",
                column: "VatNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLineItem_ItemEntityId",
                table: "PurchaseLineItem",
                column: "ItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLineItem_PurchaseEntityId",
                table: "PurchaseLineItem",
                column: "PurchaseEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BillingPartyId",
                table: "Purchases",
                column: "BillingPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ItemEntityId",
                table: "Stock",
                column: "ItemEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "PurchaseLineItem");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "BillingParties");
        }
    }
}
