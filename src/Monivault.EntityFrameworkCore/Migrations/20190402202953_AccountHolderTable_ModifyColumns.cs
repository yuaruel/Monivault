using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class AccountHolderTable_ModifyColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountIdentity",
                table: "AccountHolders",
                type: "varchar(7)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountIdentity",
                table: "AccountHolders",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(7)");
        }
    }
}
