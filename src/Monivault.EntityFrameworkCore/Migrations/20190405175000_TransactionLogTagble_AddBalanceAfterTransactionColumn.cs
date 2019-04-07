using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class TransactionLogTagble_AddBalanceAfterTransactionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfterTransaction",
                table: "TransactionLogs",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAfterTransaction",
                table: "TransactionLogs");
        }
    }
}
