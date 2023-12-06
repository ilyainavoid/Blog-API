using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Migrations
{
    /// <inheritdoc />
    public partial class SubscribersAndAdmins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityAdministrator_Community_CommunityId",
                table: "CommunityAdministrator");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityAdministrator_Users_UserId",
                table: "CommunityAdministrator");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunitySubscriber_Community_CommunityId",
                table: "CommunitySubscriber");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunitySubscriber_Users_UserId",
                table: "CommunitySubscriber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunitySubscriber",
                table: "CommunitySubscriber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityAdministrator",
                table: "CommunityAdministrator");

            migrationBuilder.RenameTable(
                name: "CommunitySubscriber",
                newName: "CommunitiesSubscribers");

            migrationBuilder.RenameTable(
                name: "CommunityAdministrator",
                newName: "CommunitiesAdministrators");

            migrationBuilder.RenameIndex(
                name: "IX_CommunitySubscriber_CommunityId",
                table: "CommunitiesSubscribers",
                newName: "IX_CommunitiesSubscribers_CommunityId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityAdministrator_CommunityId",
                table: "CommunitiesAdministrators",
                newName: "IX_CommunitiesAdministrators_CommunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunitiesSubscribers",
                table: "CommunitiesSubscribers",
                columns: new[] { "UserId", "CommunityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunitiesAdministrators",
                table: "CommunitiesAdministrators",
                columns: new[] { "UserId", "CommunityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommunitiesAdministrators_Community_CommunityId",
                table: "CommunitiesAdministrators",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunitiesAdministrators_Users_UserId",
                table: "CommunitiesAdministrators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunitiesSubscribers_Community_CommunityId",
                table: "CommunitiesSubscribers",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunitiesSubscribers_Users_UserId",
                table: "CommunitiesSubscribers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunitiesAdministrators_Community_CommunityId",
                table: "CommunitiesAdministrators");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunitiesAdministrators_Users_UserId",
                table: "CommunitiesAdministrators");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunitiesSubscribers_Community_CommunityId",
                table: "CommunitiesSubscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunitiesSubscribers_Users_UserId",
                table: "CommunitiesSubscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunitiesSubscribers",
                table: "CommunitiesSubscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunitiesAdministrators",
                table: "CommunitiesAdministrators");

            migrationBuilder.RenameTable(
                name: "CommunitiesSubscribers",
                newName: "CommunitySubscriber");

            migrationBuilder.RenameTable(
                name: "CommunitiesAdministrators",
                newName: "CommunityAdministrator");

            migrationBuilder.RenameIndex(
                name: "IX_CommunitiesSubscribers_CommunityId",
                table: "CommunitySubscriber",
                newName: "IX_CommunitySubscriber_CommunityId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunitiesAdministrators_CommunityId",
                table: "CommunityAdministrator",
                newName: "IX_CommunityAdministrator_CommunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunitySubscriber",
                table: "CommunitySubscriber",
                columns: new[] { "UserId", "CommunityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityAdministrator",
                table: "CommunityAdministrator",
                columns: new[] { "UserId", "CommunityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityAdministrator_Community_CommunityId",
                table: "CommunityAdministrator",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityAdministrator_Users_UserId",
                table: "CommunityAdministrator",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunitySubscriber_Community_CommunityId",
                table: "CommunitySubscriber",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunitySubscriber_Users_UserId",
                table: "CommunitySubscriber",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
