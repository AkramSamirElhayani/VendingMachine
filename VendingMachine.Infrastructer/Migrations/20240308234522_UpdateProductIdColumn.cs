using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendingMachine.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransaction_Product_PorductId",
                table: "InventoryTransaction");

            migrationBuilder.RenameColumn(
                name: "PorductId",
                table: "InventoryTransaction",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransaction_PorductId",
                table: "InventoryTransaction",
                newName: "IX_InventoryTransaction_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransaction_Product_ProductId",
                table: "InventoryTransaction",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransaction_Product_ProductId",
                table: "InventoryTransaction");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "InventoryTransaction",
                newName: "PorductId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransaction_ProductId",
                table: "InventoryTransaction",
                newName: "IX_InventoryTransaction_PorductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransaction_Product_PorductId",
                table: "InventoryTransaction",
                column: "PorductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
