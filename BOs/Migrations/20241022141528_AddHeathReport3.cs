using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOs.Migrations
{
    /// <inheritdoc />
    public partial class AddHeathReport3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthReport_Cages_CageID",
                table: "HealthReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthReport",
                table: "HealthReport");

            migrationBuilder.RenameTable(
                name: "HealthReport",
                newName: "HealthReports");

            migrationBuilder.RenameIndex(
                name: "IX_HealthReport_CageID",
                table: "HealthReports",
                newName: "IX_HealthReports_CageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthReports",
                table: "HealthReports",
                column: "HelthReportID");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthReports_Cages_CageID",
                table: "HealthReports",
                column: "CageID",
                principalTable: "Cages",
                principalColumn: "CageID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthReports_Cages_CageID",
                table: "HealthReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthReports",
                table: "HealthReports");

            migrationBuilder.RenameTable(
                name: "HealthReports",
                newName: "HealthReport");

            migrationBuilder.RenameIndex(
                name: "IX_HealthReports_CageID",
                table: "HealthReport",
                newName: "IX_HealthReport_CageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthReport",
                table: "HealthReport",
                column: "HelthReportID");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthReport_Cages_CageID",
                table: "HealthReport",
                column: "CageID",
                principalTable: "Cages",
                principalColumn: "CageID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
