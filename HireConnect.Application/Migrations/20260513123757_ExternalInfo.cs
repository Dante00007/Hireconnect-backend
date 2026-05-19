using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireConnect.Application.Migrations
{
    /// <inheritdoc />
    public partial class ExternalInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CandidateName",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidateName",
                table: "Applications");
        }
    }
}
