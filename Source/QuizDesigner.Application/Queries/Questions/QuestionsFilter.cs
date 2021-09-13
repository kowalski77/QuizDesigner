using System.Linq;

namespace QuizDesigner.Application.Queries.Questions
{
    public static class QuestionsFilter
    {
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
