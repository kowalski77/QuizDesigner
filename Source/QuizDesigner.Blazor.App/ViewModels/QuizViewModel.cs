using System;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public class QuizViewModel
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public string ExamName { get; set; }

        public bool Published { get; set; }
    }
}