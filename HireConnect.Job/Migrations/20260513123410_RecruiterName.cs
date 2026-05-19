using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireConnect.Job.Migrations
{
    /// <inheritdoc />
    public partial class RecruiterName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostedByName",
                table: "Jobs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedByName",
                table: "Jobs");
        }
    }
}
