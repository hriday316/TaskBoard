using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLanesAndNormalizeCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Boards_BoardId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_BoardColumnId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_BoardId",
                table: "Cards");

            migrationBuilder.CreateTable(
                name: "Lanes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lanes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lanes_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("""
                INSERT INTO "Lanes" ("Id", "BoardId", "Name", "Order", "Color", "IsDefault", "IsArchived", "CreatedAtUtc", "UpdatedAtUtc")
                SELECT "Id", "Id", 'Default', 1, NULL, TRUE, FALSE, NOW(), NULL
                FROM "Boards";
                """);

            migrationBuilder.AddColumn<Guid>(
                name: "LaneId",
                table: "Cards",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Cards"
                SET "LaneId" = "BoardId";
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "LaneId",
                table: "Cards",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Cards");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardColumnId_LaneId_Order",
                table: "Cards",
                columns: new[] { "BoardColumnId", "LaneId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_LaneId",
                table: "Cards",
                column: "LaneId");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_BoardId",
                table: "Lanes",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Lanes_LaneId",
                table: "Cards",
                column: "LaneId",
                principalTable: "Lanes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Lanes_LaneId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_BoardColumnId_LaneId_Order",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_LaneId",
                table: "Cards");

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "Cards",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Cards",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Cards" AS card
                SET "BoardId" = lane."BoardId", "WorkspaceId" = board."WorkspaceId"
                FROM "Lanes" AS lane
                INNER JOIN "Boards" AS board ON board."Id" = lane."BoardId"
                WHERE card."LaneId" = lane."Id";
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "BoardId",
                table: "Cards",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkspaceId",
                table: "Cards",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "LaneId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Lanes");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardColumnId",
                table: "Cards",
                column: "BoardColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardId",
                table: "Cards",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Boards_BoardId",
                table: "Cards",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
