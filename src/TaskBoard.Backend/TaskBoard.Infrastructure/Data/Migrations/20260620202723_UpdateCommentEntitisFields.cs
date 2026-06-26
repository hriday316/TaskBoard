using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentEntitisFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "WorkspaceMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Comments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceMemberId",
                table: "Comments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "CardAssignments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceMemberId1",
                table: "CardAssignments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_ApplicationUserId",
                table: "WorkspaceMembers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId1",
                table: "Comments",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WorkspaceMemberId",
                table: "Comments",
                column: "WorkspaceMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CardAssignments_ApplicationUserId",
                table: "CardAssignments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardAssignments_WorkspaceMemberId1",
                table: "CardAssignments",
                column: "WorkspaceMemberId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CardAssignments_Users_ApplicationUserId",
                table: "CardAssignments",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardAssignments_WorkspaceMembers_WorkspaceMemberId1",
                table: "CardAssignments",
                column: "WorkspaceMemberId1",
                principalTable: "WorkspaceMembers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId1",
                table: "Comments",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_WorkspaceMembers_WorkspaceMemberId",
                table: "Comments",
                column: "WorkspaceMemberId",
                principalTable: "WorkspaceMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceMembers_Users_ApplicationUserId",
                table: "WorkspaceMembers",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardAssignments_Users_ApplicationUserId",
                table: "CardAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CardAssignments_WorkspaceMembers_WorkspaceMemberId1",
                table: "CardAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_WorkspaceMembers_WorkspaceMemberId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceMembers_Users_ApplicationUserId",
                table: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceMembers_ApplicationUserId",
                table: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_WorkspaceMemberId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CardAssignments_ApplicationUserId",
                table: "CardAssignments");

            migrationBuilder.DropIndex(
                name: "IX_CardAssignments_WorkspaceMemberId1",
                table: "CardAssignments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "WorkspaceMembers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "WorkspaceMemberId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CardAssignments");

            migrationBuilder.DropColumn(
                name: "WorkspaceMemberId1",
                table: "CardAssignments");
        }
    }
}
