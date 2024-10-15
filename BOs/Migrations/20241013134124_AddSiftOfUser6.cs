using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOs.Migrations
{
    /// <inheritdoc />
    public partial class AddSiftOfUser6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserShifts_UserId",
                table: "UserShifts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShifts_Users_UserId",
                table: "UserShifts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShifts_Users_UserId",
                table: "UserShifts");

            migrationBuilder.DropIndex(
                name: "IX_UserShifts_UserId",
                table: "UserShifts");
        }
    }
}
