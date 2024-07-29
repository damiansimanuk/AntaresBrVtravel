using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Antares.VTravel.UI.Migrations
{
    /// <inheritdoc />
    public partial class AddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tour",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_UserId",
                table: "Tour",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tour_AspNetUsers_UserId",
                table: "Tour",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tour_AspNetUsers_UserId",
                table: "Tour");

            migrationBuilder.DropIndex(
                name: "IX_Tour_UserId",
                table: "Tour");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tour");
        }
    }
}
