using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationServices.Migrations
{
    /// <inheritdoc />
    public partial class updateHtmlDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceHTMLDocument",
                table: "Invoices",
                newName: "InvoiceHTMLDocumentPath");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                column: "PasswordHash",
                value: "$2a$11$Kw7U4nYcNBC62nzFqlVe7u3vJ4mwmTEffJHM9C5/CuYQM5lSWa5ce");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceHTMLDocumentPath",
                table: "Invoices",
                newName: "InvoiceHTMLDocument");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                column: "PasswordHash",
                value: "$2a$11$ivDQ8zRe79PTzPegJvV7AeYuPBcPveYQHC4.rgIf2RG5sxjDXB6WG");
        }
    }
}
