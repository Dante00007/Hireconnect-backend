using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireConnect.Interview.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Interviews",
                newName: "RecruiterId");

            migrationBuilder.RenameColumn(
                name: "Mode",
                table: "Interviews",
                newName: "InterviewMode");

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Interviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CandidateName",
                table: "Interviews",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Interviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Interviews",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "CandidateName",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "RecruiterId",
                table: "Interviews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "InterviewMode",
                table: "Interviews",
                newName: "Mode");
        }
    }
}
