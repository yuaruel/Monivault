using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class OtpSessionTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OtpSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PhoneNumberSentTo = table.Column<string>(type: "varchar(15)", nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    ActionProperty = table.Column<string>(type: "varchar(1000)", nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtpSessions_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtpSessions_UserId",
                table: "OtpSessions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpSessions");
        }
    }
}
