using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Antares.VTravel.UI.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class userttest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestX",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestX",
                table: "AspNetUsers");
        }
    }
}
