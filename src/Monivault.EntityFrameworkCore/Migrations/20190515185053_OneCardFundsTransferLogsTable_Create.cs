using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class OneCardFundsTransferLogsTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneCardFundsTransferLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OneCardFundsTransferLogKey = table.Column<Guid>(nullable: false),
                    AccountHolderId = table.Column<int>(nullable: false),
                    TransactionLogId = table.Column<long>(nullable: true),
                    AgentTransactionId = table.Column<string>(type: "varchar(15)", nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    ResultCode = table.Column<string>(type: "varchar(3)", nullable: false),
                    ResultDescription = table.Column<string>(type: "varchar(50)", nullable: false),
                    Action = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    BankCode = table.Column<string>(type: "varchar(5)", nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    Responsects = table.Column<string>(type: "varchar(50)", nullable: true),
                    ResponseValue = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCardFundsTransferLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCardFundsTransferLogs_AccountHolders_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalTable: "AccountHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCardFundsTransferLogs_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCardFundsTransferLogs_TransactionLogs_TransactionLogId",
                        column: x => x.TransactionLogId,
                        principalTable: "TransactionLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OneCardFundsTransferLogs_AccountHolderId",
                table: "OneCardFundsTransferLogs",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCardFundsTransferLogs_BankId",
                table: "OneCardFundsTransferLogs",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCardFundsTransferLogs_TransactionLogId",
                table: "OneCardFundsTransferLogs",
                column: "TransactionLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneCardFundsTransferLogs");
        }
    }
}
