using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapRoom.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FixPasswork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Passwork",
                table: "Accounts",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Accounts",
                newName: "Passwork");
        }
    }
}
