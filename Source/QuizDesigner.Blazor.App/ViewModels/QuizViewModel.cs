using System;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public class QuizViewModel
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public string Category { get; set; }

        public bool Published { get; set; }
    }
}