using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechBlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertisement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Click",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "View",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Click",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "View",
                table: "Advertisements");
        }
    }
}
