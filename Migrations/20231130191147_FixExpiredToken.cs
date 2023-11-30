using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Migrations
{
    /// <inheritdoc />
    public partial class FixExpiredToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpiredTokens",
                table: "ExpiredTokens",
                column: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpiredTokens",
                table: "ExpiredTokens");
        }
    }
}
