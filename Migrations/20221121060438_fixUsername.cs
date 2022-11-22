using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCourse.Migrations
{
    /// <inheritdoc />
    public partial class fixUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_userid",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Userneame",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "Characters",
                newName: "Userid");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_userid",
                table: "Characters",
                newName: "IX_Characters_Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_Userid",
                table: "Characters",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_Userid",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Userneame");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "Characters",
                newName: "userid");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_Userid",
                table: "Characters",
                newName: "IX_Characters_userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_userid",
                table: "Characters",
                column: "userid",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
