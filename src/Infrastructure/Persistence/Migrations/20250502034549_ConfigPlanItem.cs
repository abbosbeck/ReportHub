using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigPlanItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems");

            migrationBuilder.DropIndex(
                name: "IX_PlanItems_PlanId",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlanItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems",
                columns: new[] { "PlanId", "ItemId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PlanItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanItems_PlanId",
                table: "PlanItems",
                column: "PlanId");
        }
    }
}
