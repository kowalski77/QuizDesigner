using System.Collections.Generic;

namespace QuizDesigner.Application.Queries
{
    public interface IPaginatedModel<out T> where T : class
    {
        IReadOnlyList<T> Items { get; }

        int Total { get; }
    }
}