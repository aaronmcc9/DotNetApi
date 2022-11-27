using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCourse.Migrations
{
    /// <inheritdoc />
    public partial class userIdChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_Userid",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "Characters",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_Userid",
                table: "Characters",
                newName: "IX_Characters_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Characters",
                newName: "Userid");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                newName: "IX_Characters_Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_Userid",
                table: "Characters",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
