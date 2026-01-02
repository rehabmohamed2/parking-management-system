using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationServices.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                column: "PasswordHash",
                value: "$2a$11$ivDQ8zRe79PTzPegJvV7AeYuPBcPveYQHC4.rgIf2RG5sxjDXB6WG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                column: "PasswordHash",
                value: "$2a$11$3GsCZgQmM.o6yWGQYGPm.un7gb2mnzLuRRa3s7wJqPnW6nVMm6rk.");
        }
    }
}
