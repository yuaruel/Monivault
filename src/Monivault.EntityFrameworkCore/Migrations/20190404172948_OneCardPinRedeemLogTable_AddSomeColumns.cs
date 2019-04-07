using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class OneCardPinRedeemLogTable_AddSomeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestCts",
                table: "OneCardPinRedeemLogs",
                type: "varchar(25)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseCts",
                table: "OneCardPinRedeemLogs",
                type: "varchar(25)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseValue",
                table: "OneCardPinRedeemLogs",
                type: "varchar(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultCode",
                table: "OneCardPinRedeemLogs",
                type: "varchar(3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultDescription",
                table: "OneCardPinRedeemLogs",
                type: "varchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestCts",
                table: "OneCardPinRedeemLogs");

            migrationBuilder.DropColumn(
                name: "ResponseCts",
                table: "OneCardPinRedeemLogs");

            migrationBuilder.DropColumn(
                name: "ResponseValue",
                table: "OneCardPinRedeemLogs");

            migrationBuilder.DropColumn(
                name: "ResultCode",
                table: "OneCardPinRedeemLogs");

            migrationBuilder.DropColumn(
                name: "ResultDescription",
                table: "OneCardPinRedeemLogs");
        }
    }
}
