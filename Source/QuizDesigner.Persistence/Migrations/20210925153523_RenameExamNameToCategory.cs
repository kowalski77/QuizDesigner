using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizDesigner.Persistence.Migrations
{
    public partial class RenameExamNameToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExamName",
                table: "Quizzes",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Quizzes",
                newName: "ExamName");
        }
    }
}
