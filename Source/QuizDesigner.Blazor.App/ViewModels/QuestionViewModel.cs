using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizDesigner.Blazor.App.ViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; init; }

        public string Text { get; init; }

        public string Tag { get; init; }


        public bool HasAnswers => this.AnswerViewModelCollection.Any();

        public IEnumerable<AnswerViewModel> AnswerViewModelCollection { get; set; } = new List<AnswerViewModel>();
    }
}