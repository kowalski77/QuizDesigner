using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuizDesigner.Services.Queries
{
    public interface IPaginatedModel<out T> where T : class
    {
        IReadOnlyList<T> Items { get; }

        int Total { get; }

        Task PageAsync(CancellationToken cancellationToken = default);
    }
}