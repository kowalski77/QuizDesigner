using System;
using System.Linq;
using Blazorise;
using Blazorise.DataGrid;

namespace QuizDesigner.Blazor.App.Services
{
    public sealed class QueryData<T>
        where T : class
    {
        private readonly DataGridReadDataEventArgs<T> dataGridReadDataEventArgs;
        private readonly Func<string, int> getFieldValue;

        public QueryData(
            DataGridReadDataEventArgs<T> dataGridReadDataEventArgs,
            Func<string, int> getFieldValue)
        {
            this.dataGridReadDataEventArgs = dataGridReadDataEventArgs ?? throw new ArgumentNullException(nameof(dataGridReadDataEventArgs));
            this.getFieldValue = getFieldValue;
            this.Page = dataGridReadDataEventArgs.Page;
            this.PageSize = dataGridReadDataEventArgs.PageSize;

            this.Filter();
            this.Sort();
        }

        public int Page { get; }

        public int PageSize { get; }

        public int FilterField { get; private set; }

        public string FilterValue { get; private set; } = string.Empty;

        public int SortField { get; private set; }

        public bool AscendingSort { get; private set; }

        private void Sort()
        {
            var sortValue = this.dataGridReadDataEventArgs.Columns.FirstOrDefault(x => x.SortDirection != SortDirection.None);
            if (sortValue == null)
            {
                return;
            }

            this.SortField = this.getFieldValue(sortValue.Field);
            this.AscendingSort = sortValue.SortDirection == SortDirection.Ascending;
        }

        private void Filter()
        {
            var searchValue = this.dataGridReadDataEventArgs.Columns.FirstOrDefault(x => !string.IsNullOrEmpty((string)x.SearchValue));
            if (searchValue == null)
            {
                return;
            }

            this.FilterField = this.getFieldValue(searchValue.Field);
            this.FilterValue = (string)searchValue.SearchValue;
        }
    }
}