using System.Linq;

namespace QuizDesigner.Application.Queries.Quizzes
{
    public static class QuizzesFilter
    {
        public static IQueryable<QuizDto> FilterQuizzesBy(this IQueryable<QuizDto> query,
            FilterByOptions filterBy, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return query;
            }

            return filterBy switch
            {
                FilterByOptions.ByName => query.Where(x=>x.Name!.Contains(value)),
                FilterByOptions.ByExamName => query.Where(x=>x.ExamName!.Contains(value)),
                _ => query
            };
        }
    }
}