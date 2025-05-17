using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaGo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTourTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tours",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Tours",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Tours");
        }
    }
}
