using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCourse.Migrations
{
    /// <inheritdoc />
    public partial class WeaponFixName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Weapons_CharacterId",
                table: "Weapons");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CharacterId",
                table: "Weapons",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Weapons_CharacterId",
                table: "Weapons");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CharacterId",
                table: "Weapons",
                column: "CharacterId");
        }
    }
}
