using System.Collections.Generic;

namespace QuizDesigner.Services.Queries
{
    public interface IPaginatedModel<out T> where T : class
    {
        IReadOnlyList<T> Items { get; }

        int Total { get; }
    }
}