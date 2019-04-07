using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class BankTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BankKey = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    OneCardBankCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "BankKey", "Name", "OneCardBankCode" },
                values: new object[,]
                {
                    { 1, new Guid("5325647c-81e3-4ba1-a674-1f8103e2b9d3"), "Access Bank", "ACBT" },
                    { 16, new Guid("67d25915-7b11-4f48-9561-1baed0c5c87c"), "Unity Bank", "UNITY" },
                    { 15, new Guid("9dd4c693-47e2-42b2-9b4b-393edaec0eaa"), "United Bank for Africa", "UBAT" },
                    { 14, new Guid("2de38175-9d1d-4b0c-9145-f9793f2d0d15"), "Union Bank", "UBNT" },
                    { 13, new Guid("2b18849b-544b-4367-becb-f3ed13bf2dd6"), "Sterling Bank", "SBNT" },
                    { 12, new Guid("582b0c57-8f9d-426a-b894-3ec840b8b10a"), "Standard Chartered Bank", "SCBN" },
                    { 11, new Guid("d630664b-1b3b-4361-b23b-c63aef8083a9"), "Stanbic IBTC Bank", "SIBTC" },
                    { 10, new Guid("faee85ed-1964-43e5-81d8-0baae3048042"), "Skye Bank", "SKYE" },
                    { 9, new Guid("8ffc50a8-d16b-4900-9df8-2fd80d84f0a4"), "Keystone Bank", "KBNT" },
                    { 8, new Guid("437193df-fe16-496a-b9ec-182956c77c53"), "Heritage Bank", "HBNT" },
                    { 7, new Guid("862825c9-11c4-426d-8dd6-6228255c7058"), "Guaranty Trust Bank", "GTBT" },
                    { 6, new Guid("f6ce6cc9-9075-4bc9-b25b-f55e890942c0"), "FCMB", "FCMB" },
                    { 5, new Guid("ecc968d2-120e-441c-9057-db70233cf43a"), "First Bank", "FBNT" },
                    { 4, new Guid("52993498-5765-41f0-979e-d1670f246748"), "Fidelity Bank", "FDBN" },
                    { 3, new Guid("672bf4c8-c758-4043-98d4-b3de0c9bc034"), "EcoBank", "ECOB" },
                    { 2, new Guid("9746ad2d-70f5-4f80-90cf-5704e3c5d0a3"), "CitiBank", "CitBT" },
                    { 17, new Guid("84edbe8b-c308-4ebb-bfe5-559a4b0024f5"), "Wema Bank", "WEMA" },
                    { 18, new Guid("a26bec4a-2dc7-4857-8529-067944b8e77b"), "Zenith Bank", "ZBNT" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
