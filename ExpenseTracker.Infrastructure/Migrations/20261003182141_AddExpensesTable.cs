using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpensesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Categories_CategoryId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Users_UserId",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_UserId",
                table: "Expenses",
                newName: "IX_Expenses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_CategoryId",
                table: "Expenses",
                newName: "IX_Expenses_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserId",
                table: "Expense",
                newName: "IX_Expense_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_CategoryId",
                table: "Expense",
                newName: "IX_Expense_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Categories_CategoryId",
                table: "Expense",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Users_UserId",
                table: "Expense",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
