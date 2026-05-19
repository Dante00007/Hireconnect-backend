using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireConnect.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddJobAndRecruiterDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "Applied",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Applied");

            migrationBuilder.AlterColumn<string>(
                name: "CoverLetter",
                table: "Applications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Applications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecruiterName",
                table: "Applications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "RecruiterName",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Applications",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Applied",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Applied");

            migrationBuilder.AlterColumn<string>(
                name: "CoverLetter",
                table: "Applications",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
