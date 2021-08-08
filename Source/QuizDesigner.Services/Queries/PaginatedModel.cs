using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizDesigner.Services.Queries
{
    public sealed class PaginatedModel<T>
        where T : class
    {
        private readonly IQueryable<T> query;
        private readonly int pageNumber;
        private readonly int pageSize;

        public PaginatedModel(
            IQueryable<T> query,
            int page,
            int pageSize)
        {
            this.query = query;
            this.Total = query.Count();

            var numPages = (int)Math.Ceiling((double)this.Total / pageSize);
            this.pageNumber = Math.Min(Math.Max(1, page), numPages);
            this.pageSize = pageSize;
        }

        public IReadOnlyList<T> Items { get; private set; } = new List<T>();

        public int Total { get; }

        public void Page()
        {
            if (this.pageSize == 0)
            {
                throw new InvalidOperationException("Page size cannot be zero.");
            }

            var pageNumZeroStart = this.pageNumber - 1;
            if (pageNumZeroStart > 0)
            {
                this.Items = this.query.Skip(pageNumZeroStart * this.pageSize).Take(this.pageSize).ToList();
                return;
            }

            this.Items = this.query.Take(this.pageSize).ToList();
        }
    }
}