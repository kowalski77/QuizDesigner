using System.Collections.Generic;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public sealed class PageViewModel<T>
        where T : class
    {
        public IEnumerable<T> Items { get; init; }

        public int Total { get; init; }
    }
}