using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollabSessions_CodeSnippets_SnippetId",
                table: "CollabSessions");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "CollabParticipants");

            migrationBuilder.RenameColumn(
                name: "SnippetId",
                table: "CollabSessions",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_CollabSessions_SnippetId",
                table: "CollabSessions",
                newName: "IX_CollabSessions_OwnerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CollabSessions",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<Guid>(
                name: "CodeSnippetId",
                table: "CollabSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "CollabSessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CollabSessions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CollabSessions_CodeSnippetId",
                table: "CollabSessions",
                column: "CodeSnippetId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollabSessions_CodeSnippets_CodeSnippetId",
                table: "CollabSessions",
                column: "CodeSnippetId",
                principalTable: "CodeSnippets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CollabSessions_Users_OwnerId",
                table: "CollabSessions",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollabSessions_CodeSnippets_CodeSnippetId",
                table: "CollabSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_CollabSessions_Users_OwnerId",
                table: "CollabSessions");

            migrationBuilder.DropIndex(
                name: "IX_CollabSessions_CodeSnippetId",
                table: "CollabSessions");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CodeSnippetId",
                table: "CollabSessions");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "CollabSessions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CollabSessions");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "CollabSessions",
                newName: "SnippetId");

            migrationBuilder.RenameIndex(
                name: "IX_CollabSessions_OwnerId",
                table: "CollabSessions",
                newName: "IX_CollabSessions_SnippetId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CollabSessions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "CollabParticipants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CollabSessions_CodeSnippets_SnippetId",
                table: "CollabSessions",
                column: "SnippetId",
                principalTable: "CodeSnippets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
