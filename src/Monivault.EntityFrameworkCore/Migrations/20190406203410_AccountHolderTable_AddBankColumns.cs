using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class AccountHolderTable_AddBankColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "AccountHolders",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "AccountHolders",
                type: "varchar(12)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "AccountHolders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountHolders_BankId",
                table: "AccountHolders",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountHolders_Banks_BankId",
                table: "AccountHolders",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHolders_Banks_BankId",
                table: "AccountHolders");

            migrationBuilder.DropIndex(
                name: "IX_AccountHolders_BankId",
                table: "AccountHolders");

            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "AccountHolders");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "AccountHolders");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "AccountHolders");
        }
    }
}
