using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class SavingsInterestDetailTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavingsInterestDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SavingsInterestId = table.Column<long>(nullable: false),
                    TodayInterest = table.Column<decimal>(nullable: false),
                    PenaltyCharge = table.Column<decimal>(nullable: false),
                    AccruedInterestBeforeToday = table.Column<decimal>(nullable: false),
                    PrincipalBeforeTodayCalculation = table.Column<decimal>(nullable: false),
                    PrincipalAfterTodayCalculation = table.Column<decimal>(nullable: false),
                    InterestType = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavingsInterestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavingsInterestDetails_SavingsInterests_SavingsInterestId",
                        column: x => x.SavingsInterestId,
                        principalTable: "SavingsInterests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavingsInterestDetails_SavingsInterestId",
                table: "SavingsInterestDetails",
                column: "SavingsInterestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavingsInterestDetails");
        }
    }
}
