using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapRoom.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaseline : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// No changes to apply - schema is assumed to match model
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// No rollback needed
		}

	}
}
