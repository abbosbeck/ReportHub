using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanItems_Items_ItemId",
                table: "PlanItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Plans",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PlanItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PlanItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PlanItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "PlanItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PlanItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PlanItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "PlanItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "PlanItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Items_ItemId",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_ItemId",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "PlanItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanItems",
                table: "PlanItems",
                columns: new[] { "ItemId", "PlanId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlanItems_Items_ItemId",
                table: "PlanItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
