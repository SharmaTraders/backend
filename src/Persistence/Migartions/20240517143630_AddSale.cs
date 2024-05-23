using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "PurchaseLineItem",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BillingPartyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    VatAmount = table.Column<double>(type: "double precision", nullable: true),
                    TransportFee = table.Column<double>(type: "double precision", nullable: true),
                    ReceivedAmount = table.Column<double>(type: "double precision", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    InvoiceNumber = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_BillingParties_BillingPartyId",
                        column: x => x.BillingPartyId,
                        principalTable: "BillingParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleLineItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Report = table.Column<double>(type: "double precision", nullable: true),
                    SaleEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleLineItem_Items_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleLineItem_Sales_SaleEntityId",
                        column: x => x.SaleEntityId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleLineItem_ItemEntityId",
                table: "SaleLineItem",
                column: "ItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleLineItem_SaleEntityId",
                table: "SaleLineItem",
                column: "SaleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BillingPartyId",
                table: "Sales",
                column: "BillingPartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleLineItem");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "PurchaseLineItem");
        }
    }
}
