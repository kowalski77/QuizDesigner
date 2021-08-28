using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizDesigner.Application.Queries;

namespace QuizDesigner.Persistence
{
    public sealed class PaginatedModel<T> : IPaginatedModel<T> 
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

        public async Task PageAsync(CancellationToken cancellationToken = default)
        {
            if (this.pageSize == 0)
            {
                throw new InvalidOperationException("Page size cannot be zero.");
            }

            var pageNumZeroStart = this.pageNumber - 1;
            if (pageNumZeroStart > 0)
            {
                this.Items = await this.query.Skip(pageNumZeroStart * this.pageSize).Take(this.pageSize)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(true);

                return;
            }

            this.Items = await this.query.Take(this.pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);
        }
    }
}