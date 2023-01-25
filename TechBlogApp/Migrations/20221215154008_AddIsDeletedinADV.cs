using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechBlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedinADV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Advertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Advertisements");
        }
    }
}
