using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleFeed.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsletterMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsletterMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewsletterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsletterMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsletterMember_Newsletters_NewsletterId",
                        column: x => x.NewsletterId,
                        principalTable: "Newsletters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterMember_NewsletterId_UserId",
                table: "NewsletterMember",
                columns: new[] { "NewsletterId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterMember_UserId",
                table: "NewsletterMember",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsletterMember");
        }
    }
}
