using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOs.Migrations
{
    /// <inheritdoc />
    public partial class AddHeathReport2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthReport",
                columns: table => new
                {
                    HelthReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthReport", x => x.HelthReportID);
                    table.ForeignKey(
                        name: "FK_HealthReport_Cages_CageID",
                        column: x => x.CageID,
                        principalTable: "Cages",
                        principalColumn: "CageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthReport_CageID",
                table: "HealthReport",
                column: "CageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthReport");
        }
    }
}
