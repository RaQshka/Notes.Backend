using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeletedUnusedFieldsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "Notes",
                type: "TEXT",
                nullable: true);
        }
    }
}
