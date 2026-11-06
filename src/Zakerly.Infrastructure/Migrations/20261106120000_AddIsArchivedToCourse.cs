using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zakerly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsArchivedToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Courses");
        }
    }
}