using System.Linq;

namespace QuizDesigner.Application.Queries.Questions
{
    public static class QuestionsSorter
    {
        public static IQueryable<QuestionDto> SortQuestionsBy(
            this IQueryable<QuestionDto> query,
            SortByOptions options,
            bool ascending)
        {
            return (options, ascending) switch
            {
                (SortByOptions.ByTag, true) => query.OrderBy(x => x.Tag),
                (SortByOptions.ByTag, false) => query.OrderByDescending(x => x.Tag),
                (SortByOptions.ByText, true) => query.OrderBy(x => x.Text),
                (SortByOptions.ByText, false) => query.OrderByDescending(x => x.Text),
                (SortByOptions.ByCreation, true) => query.OrderBy(x => x.CreatedOn),
                _ => query.OrderByDescending(x => x.CreatedOn)
            };
        }
    }
}
