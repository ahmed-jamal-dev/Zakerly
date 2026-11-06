using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zakerly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentDueDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Assignments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Assignments");
        }
    }
}
