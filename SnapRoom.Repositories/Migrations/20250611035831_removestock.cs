using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapRoom.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class removestock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Furnitures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Furnitures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
