using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProiectDAW.Data.Migrations
{
    /// <inheritdoc />
    public partial class poza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "Posts");
        }
    }
}
