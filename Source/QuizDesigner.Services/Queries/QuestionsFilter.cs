using System.Linq;

namespace QuizDesigner.Services.Queries
{
    public static class QuestionsFilter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Justification = "EF Core  client evaluation exception")]
        public static IQueryable<QuestionDto> FilterQuestionsBy(this IQueryable<QuestionDto> query,
            FilterByOptions filterBy, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return query;
            }

            return filterBy switch
            {
                FilterByOptions.ByTag => query.Where(x => x.Tag!.Contains(value)),
                FilterByOptions.ByText => query.Where(x => x.Text!.Contains(value)),
                _ => query
            };
        }
    }
}
