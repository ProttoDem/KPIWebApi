using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskWebApiLab.Migrations
{
    /// <inheritdoc />
    public partial class DueTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueTime",
                table: "Goals",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueTime",
                table: "Goals");
        }
    }
}
