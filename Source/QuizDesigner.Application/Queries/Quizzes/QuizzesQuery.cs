namespace QuizDesigner.Application.Queries.Quizzes
{
    public class QuizzesQuery
    {
        public FilterByOptions FilterByOptions { get; init; }

        public SortByOptions SortByOptions { get; init; }

        public string FilterValue { get; init; } = string.Empty;

        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        public bool AscendingSort { get; init; }
    }
}