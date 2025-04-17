using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Items",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Invoices",
                newName: "CurrencyCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Items",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Invoices",
                newName: "Currency");
        }
    }
}
