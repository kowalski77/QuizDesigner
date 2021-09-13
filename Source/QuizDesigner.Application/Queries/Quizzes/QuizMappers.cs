using System.Linq;

namespace QuizDesigner.Application.Queries.Quizzes
{
    public static class QuizMappers
    {
        public static IQueryable<QuizDto> Map(this IQueryable<Quiz> source)
        {
            return source.Select(x => new QuizDto
            {
                Id = x.Id,
                Name = x.Name,
                ExamName = x.ExamName,
                IsPublished = x.IsPublished
            });
        }
    }
}