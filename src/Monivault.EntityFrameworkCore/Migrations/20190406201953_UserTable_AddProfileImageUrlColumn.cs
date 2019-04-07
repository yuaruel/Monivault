using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class UserTable_AddProfileImageUrlColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AbpUsers",
                type: "varchar(150)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AbpUsers");
        }
    }
}
