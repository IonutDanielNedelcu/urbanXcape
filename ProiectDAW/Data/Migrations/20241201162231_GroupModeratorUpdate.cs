using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProiectDAW.Data.Migrations
{
    /// <inheritdoc />
    public partial class GroupModeratorUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ModeratorId",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ModeratorId",
                table: "Groups",
                column: "ModeratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_ModeratorId",
                table: "Groups",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_ModeratorId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ModeratorId",
                table: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "ModeratorId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
