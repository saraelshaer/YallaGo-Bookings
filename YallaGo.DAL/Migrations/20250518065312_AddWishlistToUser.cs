using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaGo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_wishLists_UserId",
                table: "wishLists");

            migrationBuilder.CreateIndex(
                name: "IX_wishLists_UserId",
                table: "wishLists",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_wishLists_UserId",
                table: "wishLists");

            migrationBuilder.CreateIndex(
                name: "IX_wishLists_UserId",
                table: "wishLists",
                column: "UserId");
        }
    }
}
