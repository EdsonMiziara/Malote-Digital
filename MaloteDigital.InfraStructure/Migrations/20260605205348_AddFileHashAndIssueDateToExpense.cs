using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaloteDigital.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFileHashAndIssueDateToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileHash",
                table: "Expenses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileHash",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Expenses");
        }
    }
}
