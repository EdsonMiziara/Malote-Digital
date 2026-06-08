using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaloteDigital.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActualPaymentDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualPaymentDate",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualPaymentDate",
                table: "Expenses");
        }
    }
}
