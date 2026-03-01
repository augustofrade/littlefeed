using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleFeed.Migrations
{
    /// <inheritdoc />
    public partial class DraftArticleProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPostDate",
                table: "Newsletters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Articles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPostDate",
                table: "Newsletters");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Articles");
        }
    }
}
