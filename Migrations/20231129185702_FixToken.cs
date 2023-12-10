using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlogApi.Migrations
{
    /// <inheritdoc />
    public partial class FixToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpiredTokens_Users_UserId",
                table: "ExpiredTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpiredTokens",
                table: "ExpiredTokens");

            migrationBuilder.DropIndex(
                name: "IX_ExpiredTokens_UserId",
                table: "ExpiredTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExpiredTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExpiredTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ExpiredTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ExpiredTokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpiredTokens",
                table: "ExpiredTokens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExpiredTokens_UserId",
                table: "ExpiredTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpiredTokens_Users_UserId",
                table: "ExpiredTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
