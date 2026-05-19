using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireConnect.Profile.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileImagesAndCompanyLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoUrl",
                table: "Recruiters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Candidates",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogoUrl",
                table: "Recruiters");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Candidates");
        }
    }
}
