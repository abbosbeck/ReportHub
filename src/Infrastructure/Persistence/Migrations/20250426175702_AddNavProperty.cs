using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNavProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlanItems_ItemId",
                table: "PlanItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanItems_Items_ItemId",
                table: "PlanItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanItems_Items_ItemId",
                table: "PlanItems");

            migrationBuilder.DropIndex(
                name: "IX_PlanItems_ItemId",
                table: "PlanItems");
        }
    }
}
