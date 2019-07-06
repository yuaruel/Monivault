using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monivault.Migrations
{
    public partial class TaxTypeTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TaxTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Companies Income Tax (CIT)" },
                    { 2, "Petroleum Profit Tax(PPT)" },
                    { 3, "Education Tax (EDT)" },
                    { 4, "Value Added Tax (VAT)" },
                    { 5, "Capital Gains Tax (CGT)" },
                    { 6, "Pre-Operational Levy" },
                    { 7, "Pay As You Earn (PAYE)" },
                    { 8, "Personal Income Tax (PIT)" },
                    { 9, "Interest" },
                    { 10, "Withholding Tax (WHT)" },
                    { 11, "Stamp Duties (STD) - New Business Registration" },
                    { 12, "Stamp Duties (STD) - Registered Company" },
                    { 13, "National Information Technology Development Fund (NITDF) Levy" },
                    { 14, "Annual Luxury Surcharge" },
                    { 15, "Foreign Travel Surcharge" },
                    { 16, "Penalties" },
                    { 17, "National Agency for Science and Engineering Infrastructure (NASENI) Levy" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxTypes");
        }
    }
}
