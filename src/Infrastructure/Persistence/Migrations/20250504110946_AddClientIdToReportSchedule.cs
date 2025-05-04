using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClientIdToReportSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "ReportSchedules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReportSchedules_ClientId",
                table: "ReportSchedules",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportSchedules_Clients_ClientId",
                table: "ReportSchedules",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportSchedules_Clients_ClientId",
                table: "ReportSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ReportSchedules_ClientId",
                table: "ReportSchedules");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ReportSchedules");
        }
    }
}
