using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaloteDigital.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailedDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailedDescription",
                table: "Expenses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailedDescription",
                table: "Expenses");
        }
    }
}
