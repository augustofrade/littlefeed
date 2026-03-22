using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleFeed.Migrations
{
    /// <inheritdoc />
    public partial class NewsletterSubscriptionIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsletterSubscriptions_NewsletterId_UserId",
                table: "NewsletterSubscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterSubscriptions_NewsletterId_GuestEmail",
                table: "NewsletterSubscriptions",
                columns: new[] { "NewsletterId", "GuestEmail" },
                unique: true,
                filter: "[GuestEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterSubscriptions_NewsletterId_UserId",
                table: "NewsletterSubscriptions",
                columns: new[] { "NewsletterId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsletterSubscriptions_NewsletterId_GuestEmail",
                table: "NewsletterSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_NewsletterSubscriptions_NewsletterId_UserId",
                table: "NewsletterSubscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterSubscriptions_NewsletterId_UserId",
                table: "NewsletterSubscriptions",
                columns: new[] { "NewsletterId", "UserId" });
        }
    }
}
