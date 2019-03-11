using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class VerificationCodeTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignUpVerificationCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpVerificationCodes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignUpVerificationCodes");
        }
    }
}
