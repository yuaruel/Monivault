using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class TaxPaymentTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxPayments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TaxPaymentKey = table.Column<Guid>(nullable: false),
                    Tin = table.Column<string>(type: "varchar(20)", nullable: false),
                    FullName = table.Column<string>(type: " varchar(100)", nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TaxPeriod = table.Column<DateTimeOffset>(nullable: false),
                    ReconcilliationPvNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    EmailAddress = table.Column<string>(type: "varchar(50)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", nullable: true),
                    AccountHolderId = table.Column<int>(nullable: false),
                    TaxTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxPayments_AccountHolders_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalTable: "AccountHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxPayments_TaxTypes_TaxTypeId",
                        column: x => x.TaxTypeId,
                        principalTable: "TaxTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxPayments_AccountHolderId",
                table: "TaxPayments",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxPayments_TaxTypeId",
                table: "TaxPayments",
                column: "TaxTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxPayments");
        }
    }
}
