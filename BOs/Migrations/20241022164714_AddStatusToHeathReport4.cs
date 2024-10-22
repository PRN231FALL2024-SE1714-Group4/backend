using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOs.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToHeathReport4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "HealthReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HealthReports_UserID",
                table: "HealthReports",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthReports_Users_UserID",
                table: "HealthReports",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthReports_Users_UserID",
                table: "HealthReports");

            migrationBuilder.DropIndex(
                name: "IX_HealthReports_UserID",
                table: "HealthReports");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "HealthReports");
        }
    }
}
