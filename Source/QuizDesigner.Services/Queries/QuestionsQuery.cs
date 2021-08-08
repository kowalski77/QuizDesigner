namespace QuizDesigner.Services.Queries
{
    public sealed class QuestionsQuery
    {
        public QuestionsQuery(FilterByOptions filterByOptions, SortByOptions sortByOptions,
            string filterValue, int pageNumber, int pageSize, bool ascendingSort)
        {
            this.FilterByOptions = filterByOptions;
            this.SortByOptions = sortByOptions;
            this.FilterValue = filterValue;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.AscendingSort = ascendingSort;
        }

        public FilterByOptions FilterByOptions { get; }

        public SortByOptions SortByOptions { get; }

        public string FilterValue { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public bool AscendingSort { get; }
    }
}