using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhihu.QuestionService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByType",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByType",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByType",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedByType",
                table: "Answers");
        }
    }
}
