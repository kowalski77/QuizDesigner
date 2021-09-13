using System.Linq;

namespace QuizDesigner.Application.Queries.Quizzes
{
    public static class QuizzesSorter
    {
        public static IQueryable<QuizDto> SortQuizzesBy(
            this IQueryable<QuizDto> query,
            SortByOptions options,
            bool ascending)
        {
            return (options, ascending) switch
            {
                (SortByOptions.ByName, true) => query.OrderBy(x => x.Name),
                (SortByOptions.ByName, false) => query.OrderByDescending(x => x.Name),
                (SortByOptions.ByExamName, true) => query.OrderBy(x => x.ExamName),
                (SortByOptions.ByExamName, false) => query.OrderByDescending(x => x.ExamName),
                (SortByOptions.ByPublished, true) => query.OrderBy(x => x.IsPublished),
                (SortByOptions.ByPublished, false) => query.OrderByDescending(x => x.IsPublished),
                _ => query.OrderByDescending(x => x.IsPublished)
            };
        }
    }
}