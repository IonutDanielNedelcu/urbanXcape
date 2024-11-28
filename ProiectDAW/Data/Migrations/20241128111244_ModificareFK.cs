using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProiectDAW.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModificareFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLocations_Locations_IdLocation",
                table: "PostLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLocations_Posts_IdPost",
                table: "PostLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratigs",
                table: "Ratigs");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Ratigs");

            migrationBuilder.DropColumn(
                name: "IdGroup",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IdPost",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "IdPost",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "IdLocation",
                table: "Ratigs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Posts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdLocation",
                table: "PostLocations",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "IdPost",
                table: "PostLocations",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostLocations_IdLocation",
                table: "PostLocations",
                newName: "IX_PostLocations_LocationId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Ratigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatingCounter",
                table: "Locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratigs",
                table: "Ratigs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLocations_Locations_LocationId",
                table: "PostLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLocations_Posts_PostId",
                table: "PostLocations",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLocations_Locations_LocationId",
                table: "PostLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLocations_Posts_PostId",
                table: "PostLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratigs",
                table: "Ratigs");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ratigs",
                newName: "IdLocation");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Posts",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "PostLocations",
                newName: "IdLocation");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostLocations",
                newName: "IdPost");

            migrationBuilder.RenameIndex(
                name: "IX_PostLocations_LocationId",
                table: "PostLocations",
                newName: "IX_PostLocations_IdLocation");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "IdUser");

            migrationBuilder.AlterColumn<int>(
                name: "IdLocation",
                table: "Ratigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "Ratigs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdGroup",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPost",
                table: "Media",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<float>(
                name: "RatingCounter",
                table: "Locations",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdPost",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratigs",
                table: "Ratigs",
                column: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLocations_Locations_IdLocation",
                table: "PostLocations",
                column: "IdLocation",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLocations_Posts_IdPost",
                table: "PostLocations",
                column: "IdPost",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
