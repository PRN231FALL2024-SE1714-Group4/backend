using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOs.Migrations
{
    /// <inheritdoc />
    public partial class ResetHistory3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                column: "HistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_AnimalID",
                table: "Histories",
                column: "AnimalID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_AnimalID",
                table: "Histories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                columns: new[] { "AnimalID", "CageID" });
        }
    }
}
