using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddressDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Registers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Registers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Registers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Registers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Registers");
        }
    }
}
