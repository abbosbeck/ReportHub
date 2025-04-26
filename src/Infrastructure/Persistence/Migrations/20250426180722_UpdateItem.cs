using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Items_ItemId",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_ItemId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Plans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Plans",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_ItemId",
                table: "Plans",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Items_ItemId",
                table: "Plans",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
