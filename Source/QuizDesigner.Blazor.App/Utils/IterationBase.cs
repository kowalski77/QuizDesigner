using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace QuizDesigner.Blazor.App.Utils
{
    public class IterationBase<T> : ComponentBase
    {
        [Parameter]
        public IEnumerable<T> Items { get; set; }

        [Parameter]
        public RenderFragment<T> ChildContent { get; set; }
    }
}