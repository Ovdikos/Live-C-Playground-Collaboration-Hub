using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDelete_SessionEditHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionEditHistories_Users_EditedByUserId",
                table: "SessionEditHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionEditHistories_Users_EditedByUserId",
                table: "SessionEditHistories",
                column: "EditedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionEditHistories_Users_EditedByUserId",
                table: "SessionEditHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionEditHistories_Users_EditedByUserId",
                table: "SessionEditHistories",
                column: "EditedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
