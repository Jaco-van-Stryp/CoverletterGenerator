using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoverletterGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddContactDetailsToCV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CVs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHub",
                table: "CVs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "CVs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "CVs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CVs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "CVs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "CVs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "GitHub",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "CVs");
        }
    }
}
