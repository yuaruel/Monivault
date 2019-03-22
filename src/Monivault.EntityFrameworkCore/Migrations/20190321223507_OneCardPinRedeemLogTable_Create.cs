using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class OneCardPinRedeemLogTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneCardPinRedeemLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OneCardPinRedeemKey = table.Column<Guid>(nullable: false),
                    AccountHolderId = table.Column<int>(nullable: false),
                    TransactionLogId = table.Column<long>(nullable: true),
                    AgentTransactionId = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    PinNo = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    SerialNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    ServiceId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Comments = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    VendorCode = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    ProductCode = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCardPinRedeemLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCardPinRedeemLogs_AccountHolders_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalTable: "AccountHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCardPinRedeemLogs_TransactionLogs_TransactionLogId",
                        column: x => x.TransactionLogId,
                        principalTable: "TransactionLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OneCardPinRedeemLogs_AccountHolderId",
                table: "OneCardPinRedeemLogs",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCardPinRedeemLogs_TransactionLogId",
                table: "OneCardPinRedeemLogs",
                column: "TransactionLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneCardPinRedeemLogs");
        }
    }
}
