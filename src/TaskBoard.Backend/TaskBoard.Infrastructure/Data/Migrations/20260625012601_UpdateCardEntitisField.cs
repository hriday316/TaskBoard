using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoard.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardEntitisField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Cards"
                ALTER COLUMN "Attachments" TYPE text[]
                USING CASE
                    WHEN "Attachments" IS NULL THEN NULL
                    ELSE ARRAY["Attachments"]
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Cards"
                ALTER COLUMN "Attachments" TYPE text
                USING CASE
                    WHEN "Attachments" IS NULL THEN NULL
                    ELSE array_to_string("Attachments", ',')
                END;
                """);
        }
    }
}
