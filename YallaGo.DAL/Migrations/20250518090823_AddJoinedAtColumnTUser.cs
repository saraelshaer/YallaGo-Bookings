using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaGo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinedAtColumnTUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "AspNetUsers");
        }
    }
}
