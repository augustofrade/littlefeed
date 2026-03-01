using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleFeed.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsletterMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsletterMember_AspNetUsers_UserId",
                table: "NewsletterMember");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsletterMember_Newsletters_NewsletterId",
                table: "NewsletterMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsletterMember",
                table: "NewsletterMember");

            migrationBuilder.RenameTable(
                name: "NewsletterMember",
                newName: "NewsletterMembers");

            migrationBuilder.RenameIndex(
                name: "IX_NewsletterMember_UserId",
                table: "NewsletterMembers",
                newName: "IX_NewsletterMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsletterMember_NewsletterId_UserId",
                table: "NewsletterMembers",
                newName: "IX_NewsletterMembers_NewsletterId_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsletterMembers",
                table: "NewsletterMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsletterMembers_AspNetUsers_UserId",
                table: "NewsletterMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsletterMembers_Newsletters_NewsletterId",
                table: "NewsletterMembers",
                column: "NewsletterId",
                principalTable: "Newsletters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsletterMembers_AspNetUsers_UserId",
                table: "NewsletterMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsletterMembers_Newsletters_NewsletterId",
                table: "NewsletterMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsletterMembers",
                table: "NewsletterMembers");

            migrationBuilder.RenameTable(
                name: "NewsletterMembers",
                newName: "NewsletterMember");

            migrationBuilder.RenameIndex(
                name: "IX_NewsletterMembers_UserId",
                table: "NewsletterMember",
                newName: "IX_NewsletterMember_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsletterMembers_NewsletterId_UserId",
                table: "NewsletterMember",
                newName: "IX_NewsletterMember_NewsletterId_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsletterMember",
                table: "NewsletterMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsletterMember_AspNetUsers_UserId",
                table: "NewsletterMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsletterMember_Newsletters_NewsletterId",
                table: "NewsletterMember",
                column: "NewsletterId",
                principalTable: "Newsletters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
