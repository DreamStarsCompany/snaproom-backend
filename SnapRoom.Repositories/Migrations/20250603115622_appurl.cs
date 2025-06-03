using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapRoom.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class appurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUrl",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUrl",
                table: "Accounts");
        }
    }
}
