using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class UserTable_AddUserKeyColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserKey",
                table: "AbpUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserKey",
                table: "AbpUsers");
        }
    }
}
