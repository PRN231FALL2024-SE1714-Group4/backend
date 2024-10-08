using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOs.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureWorkAndReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Cages_CageID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Areas_AreaID",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CageID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CageID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Clean",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Feed",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "Works",
                newName: "CageID");

            migrationBuilder.AlterColumn<int>(
                name: "Shift",
                table: "Works",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaID",
                table: "Works",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Mission",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Animals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Breed",
                table: "Animals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Works_CageID",
                table: "Works",
                column: "CageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Areas_AreaID",
                table: "Works",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Cages_CageID",
                table: "Works",
                column: "CageID",
                principalTable: "Cages",
                principalColumn: "CageID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_Areas_AreaID",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Cages_CageID",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_CageID",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Mission",
                table: "Works");

            migrationBuilder.RenameColumn(
                name: "CageID",
                table: "Works",
                newName: "RoleID");

            migrationBuilder.AlterColumn<string>(
                name: "Shift",
                table: "Works",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaID",
                table: "Works",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CageID",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Clean",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Feed",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Animals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Breed",
                table: "Animals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CageID",
                table: "Reports",
                column: "CageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Cages_CageID",
                table: "Reports",
                column: "CageID",
                principalTable: "Cages",
                principalColumn: "CageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Areas_AreaID",
                table: "Works",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
