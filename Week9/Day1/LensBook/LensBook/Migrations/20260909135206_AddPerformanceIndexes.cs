using Microsoft.EntityFrameworkCore.Migrations;



namespace LensBook.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_PhotographerId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_CreatedAt",
                table: "Notifications",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PhotographerId_StartTime",
                table: "Bookings",
                columns: new[] { "PhotographerId", "StartTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_CreatedAt",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PhotographerId_StartTime",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PhotographerId",
                table: "Bookings",
                column: "PhotographerId");
        }
    }
}
