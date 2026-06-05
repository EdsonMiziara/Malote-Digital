using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaloteDigital.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expenses_CondominiumId",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "PdfUrl",
                table: "Expenses",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expense_Unique_Condo_Amount_DueDate",
                table: "Expenses",
                columns: new[] { "CondominiumId", "Amount", "DueDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expense_Unique_Condo_Amount_DueDate",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "PdfUrl",
                table: "Expenses");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CondominiumId",
                table: "Expenses",
                column: "CondominiumId");
        }
    }
}
