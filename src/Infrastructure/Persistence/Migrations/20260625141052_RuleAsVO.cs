using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RuleAsVO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_matching_rules",
                table: "matching_rules");

            migrationBuilder.DropIndex(
                name: "IX_matching_rules_category_id",
                table: "matching_rules");

            migrationBuilder.DropColumn(
                name: "id",
                table: "matching_rules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_matching_rules",
                table: "matching_rules",
                columns: new[] { "category_id", "keyword" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_matching_rules",
                table: "matching_rules");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "matching_rules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_matching_rules",
                table: "matching_rules",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_matching_rules_category_id",
                table: "matching_rules",
                column: "category_id");
        }
    }
}
