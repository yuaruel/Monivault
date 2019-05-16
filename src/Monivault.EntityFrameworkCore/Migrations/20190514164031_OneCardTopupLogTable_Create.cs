using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class OneCardTopupLogTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneCardTopupLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OneCardTopupLogKey = table.Column<Guid>(nullable: false),
                    AccountHolderId = table.Column<int>(nullable: false),
                    TransactionLogId = table.Column<long>(nullable: true),
                    AgentTransactionId = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Destination = table.Column<string>(type: "varchar(11)", nullable: false),
                    MobileNumber = table.Column<string>(type: "varchar(11)", nullable: true),
                    Type = table.Column<string>(type: "varchar(20)", nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    ProductCode = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    ResultCode = table.Column<string>(type: "varchar(3)", nullable: true),
                    ResultDescription = table.Column<string>(type: "varchar(50)", nullable: true),
                    ResponseValue = table.Column<string>(type: "varchar(2)", nullable: true),
                    RequestCts = table.Column<string>(type: "varchar(25)", nullable: true),
                    ResponseCts = table.Column<string>(type: "varchar(25)", nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCardTopupLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCardTopupLogs_AccountHolders_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalTable: "AccountHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCardTopupLogs_TransactionLogs_TransactionLogId",
                        column: x => x.TransactionLogId,
                        principalTable: "TransactionLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OneCardTopupLogs_AccountHolderId",
                table: "OneCardTopupLogs",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCardTopupLogs_TransactionLogId",
                table: "OneCardTopupLogs",
                column: "TransactionLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneCardTopupLogs");
        }
    }
}
