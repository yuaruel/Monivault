using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class AccountHolderTable_AddAccountOfficerColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountOfficerId",
                table: "AccountHolders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountHolders_AccountOfficerId",
                table: "AccountHolders",
                column: "AccountOfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountHolders_AbpUsers_AccountOfficerId",
                table: "AccountHolders",
                column: "AccountOfficerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHolders_AbpUsers_AccountOfficerId",
                table: "AccountHolders");

            migrationBuilder.DropIndex(
                name: "IX_AccountHolders_AccountOfficerId",
                table: "AccountHolders");

            migrationBuilder.DropColumn(
                name: "AccountOfficerId",
                table: "AccountHolders");
        }
    }
}
